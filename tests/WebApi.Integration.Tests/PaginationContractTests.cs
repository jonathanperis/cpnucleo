using System.Net.Http.Json;
using System.Text.Json;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer.Contracts.Commands.Appointment;
using GrpcServer.Contracts.Commands.Assignment;
using GrpcServer.Contracts.Commands.Project;
using GrpcServer.Contracts.Commands.Workflow;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Integration.Tests;

/// <summary>
/// Invalid paging is a validation failure on both transports, whatever produced the request:
/// REST query binding, the typed gRPC client, or a hand-crafted gRPC message. Relation and date
/// filters narrow lists on both transports, never past what the caller may see, and filters a
/// resource can't apply are rejected rather than ignored.
/// </summary>
[Collection("Database")]
public class PaginationContractTests(WebAppFixture app)
{
    private static CancellationToken Cancellation => TestContext.Current.CancellationToken;

    public static TheoryData<string, string> InvalidQueries => new()
    {
        { "pageSize=101", "pageSize" },
        { "pageSize=0", "pageSize" },
        { "pageNumber=0", "pageNumber" },
        { "ids=not-a-uuid", "ids" },
        { $"search={new string('x', 129)}", "search" },
        { "projectId=not-a-uuid", "projectId" },
        { "workflowId=00000000-0000-0000-0000-00000000000g", "workflowId" }
    };

    [Theory]
    [MemberData(nameof(InvalidQueries))]
    public async Task Rest_RejectsInvalidPagingWithTheOffendingField(string query, string field)
    {
        var response = await app.Client.GetAsync($"/api/workflows?{query}", Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("errors").TryGetProperty(field, out _).ShouldBeTrue();
    }

    [Theory]
    [InlineData(101, null, "PageSize must be between 1 and 100.")]
    [InlineData(10, "not-a-uuid", "Ids must contain at most 100 comma-separated UUIDs.")]
    public async Task TypedGrpcCommands_RejectInvalidPagingAsInvalidArgument(int pageSize, string? ids, string message)
    {
        var command = new ListWorkflowsCommand { Pagination = new PaginationParams { PageSize = pageSize, Ids = ids } };

        var error = await Should.ThrowAsync<RpcException>(() => command.RemoteExecuteAsync(WebAppFixture.GrpcOptions()));

        error.StatusCode.ShouldBe(StatusCode.InvalidArgument);
        error.Status.Detail.ShouldBe(message);
    }

    public static TheoryData<string, Dictionary<string, object>, StatusCode> RawMessages => new()
    {
        { "page size above the bound", new() { ["Pagination"] = new Dictionary<string, object> { ["PageSize"] = 101 } }, StatusCode.InvalidArgument },
        { "malformed ids", new() { ["Pagination"] = new Dictionary<string, object> { ["Ids"] = "nope" } }, StatusCode.InvalidArgument },
        { "missing pagination", new(), StatusCode.InvalidArgument },
        { "valid paging", new() { ["Pagination"] = new Dictionary<string, object> { ["PageSize"] = 5 } }, StatusCode.OK }
    };

    [Theory]
    [MemberData(nameof(RawMessages))]
    public async Task HandCraftedGrpcMessages_FailAsValidationNotAsServerErrors(string scenario, Dictionary<string, object> body, StatusCode expected)
    {
        // A client that bypasses the typed contracts sends whatever it wants; serialize like FastEndpoints
        // Remote Messaging (MessagePack, contractless) and call the command's route directly.
        var route = app.GrpcServices.GetServices<EndpointDataSource>().SelectMany(source => source.Endpoints).OfType<RouteEndpoint>()
            .Single(endpoint => endpoint.RoutePattern.RawText == $"/{typeof(ListWorkflowsCommand).FullName}/");
        var method = new Method<byte[], byte[]>(MethodType.Unary, typeof(ListWorkflowsCommand).FullName!, string.Empty,
            Marshallers.Create(bytes => bytes, bytes => bytes), Marshallers.Create(bytes => bytes, bytes => bytes));
        var payload = MessagePackSerializer.Serialize(body, MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance), Cancellation);
        using var channel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions { HttpHandler = app.GrpcHandler() });

        var status = StatusCode.OK;
        try
        {
            await channel.CreateCallInvoker().AsyncUnaryCall(method, null, WebAppFixture.GrpcOptions(), payload);
        }
        catch (RpcException error)
        {
            status = error.StatusCode;
        }

        route.ShouldNotBeNull();
        status.ShouldBe(expected, scenario);
    }

    public static TheoryData<string, string, string> InvalidFilters => new()
    {
        { "appointments?assignmentId=nope", "assignmentId", "AssignmentId must be a UUID." },
        { "appointments?dateFrom=yesterday", "dateFrom", "DateFrom must be an ISO-8601 date and time." },
        { "appointments?dateTo=2031-02-30", "dateTo", "DateTo must be an ISO-8601 date and time." },
        { "assignments?dateFrom=2031-02-01&dateTo=2031-01-01", "dateTo", "DateTo must be on or after DateFrom." },
        { $"projects?userId={Guid.NewGuid()}", "userId", "Projects cannot be filtered by userId." },
        { "organizations?dateFrom=2031-01-01", "dateFrom", "Organizations cannot be filtered by dateFrom." },
        { $"impediments?projectId={Guid.NewGuid()}", "projectId", "Impediments cannot be filtered by projectId." },
        { $"users?projectId={Guid.NewGuid()}", "projectId", "Users cannot be filtered by projectId." },
        { $"userProjects?assignmentId={Guid.NewGuid()}", "assignmentId", "UserProjects cannot be filtered by assignmentId." }
    };

    [Theory]
    [MemberData(nameof(InvalidFilters))]
    public async Task Rest_RejectsInvalidOrUnsupportedFiltersWithTheOffendingField(string query, string field, string message)
    {
        var response = await app.Client.GetAsync($"/api/{query}", Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var errors = (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("errors");
        errors.GetProperty(field).EnumerateArray().Select(error => error.GetString()).ShouldContain(message);

        // Live listings validate the same request before the stream starts.
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/{query}");
        request.Headers.Accept.ParseAdd("text/event-stream");
        using var stream = await app.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Cancellation);
        stream.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GrpcLists_RejectUnsupportedFiltersAsInvalidArgument()
    {
        var command = new ListProjectsCommand { Pagination = new PaginationParams { UserId = Guid.NewGuid().ToString() } };

        var error = await Should.ThrowAsync<RpcException>(() => command.RemoteExecuteAsync(WebAppFixture.GrpcOptions()));

        error.StatusCode.ShouldBe(StatusCode.InvalidArgument);
        error.Status.Detail.ShouldBe("Projects cannot be filtered by userId.");
        (await Should.ThrowAsync<RpcException>(() => new ListAppointmentsCommand { Pagination = new PaginationParams { DateFrom = "yesterday" } }
            .RemoteExecuteAsync(WebAppFixture.GrpcOptions()))).Status.Detail.ShouldBe("DateFrom must be an ISO-8601 date and time.");
    }

    [Fact]
    public async Task RelationFilters_NarrowTheList_WithSearchAndPaging_OnBothTransports()
    {
        var mine = await app.CreateGraphAsync();
        var other = await app.CreateGraphAsync();
        await AddAppointmentsAsync(
            (mine.Assignment.Id, "Filter alpha one", DateTime.UtcNow),
            (mine.Assignment.Id, "Filter alpha two", DateTime.UtcNow),
            (mine.Assignment.Id, "Unrelated", DateTime.UtcNow),
            (other.Assignment.Id, "Filter alpha three", DateTime.UtcNow));

        var page = await ListAsync($"appointments?assignmentId={mine.Assignment.Id}&search=alpha&pageSize=1&sortColumn=Description");
        page.GetProperty("totalCount").GetInt32().ShouldBe(2);
        page.GetProperty("data").GetArrayLength().ShouldBe(1);
        page.GetProperty("data")[0].GetProperty("description").GetString().ShouldBe("Filter alpha one");
        (await ListAsync($"appointments?assignmentId={mine.Assignment.Id}&search=alpha&pageSize=1&pageNumber=2&sortColumn=Description"))
            .GetProperty("data")[0].GetProperty("description").GetString().ShouldBe("Filter alpha two");
        (await ListAsync($"appointments?assignmentId={mine.Assignment.Id}&userId={mine.User.Id}")).GetProperty("totalCount").GetInt32().ShouldBe(3);
        (await ListAsync($"appointments?assignmentId={mine.Assignment.Id}&userId={other.User.Id}")).GetProperty("totalCount").GetInt32().ShouldBe(0);

        var ids = $"{mine.Project.Id},{other.Project.Id}";
        (await ListAsync($"projects?ids={ids}&organizationId={mine.Organization.Id}")).GetProperty("data")[0].GetProperty("id").GetGuid().ShouldBe(mine.Project.Id);
        (await ListAsync($"assignments?projectId={other.Project.Id}")).GetProperty("data")[0].GetProperty("id").GetGuid().ShouldBe(other.Assignment.Id);
        (await ListAsync($"assignments?workflowId={mine.Workflow.Id}&userId={mine.User.Id}")).GetProperty("totalCount").GetInt32().ShouldBe(1);
        (await ListAsync($"assignments?projectId={mine.Project.Id}&workflowId={other.Workflow.Id}")).GetProperty("totalCount").GetInt32().ShouldBe(0, "filters combine with AND");

        var grpc = await new ListAppointmentsCommand
        {
            Pagination = new PaginationParams { AssignmentId = mine.Assignment.Id.ToString(), Search = "alpha", PageSize = 1 }
        }.RemoteExecuteAsync(WebAppFixture.GrpcOptions());
        grpc.Result.TotalCount.ShouldBe(2);
        grpc.Result.Data!.Count().ShouldBe(1);
        (await new ListProjectsCommand { Pagination = new PaginationParams { Ids = ids, OrganizationId = other.Organization.Id.ToString() } }
            .RemoteExecuteAsync(WebAppFixture.GrpcOptions())).Result.Data!.Single()!.Id.ShouldBe(other.Project.Id);
    }

    [Fact]
    public async Task DateRanges_SelectAppointmentsByDay_AndAssignmentsByOverlap()
    {
        var graph = await app.CreateGraphAsync();
        await AddAppointmentsAsync(
            (graph.Assignment.Id, "January 10", Utc(2031, 1, 10)),
            (graph.Assignment.Id, "January 20", Utc(2031, 1, 20)),
            (graph.Assignment.Id, "February 1", Utc(2031, 2, 1)));

        async Task<string[]> AppointmentsAsync(string range) =>
            (await ListAsync($"appointments?assignmentId={graph.Assignment.Id}&{range}&sortColumn=KeepDate&pageSize=100"))
                .GetProperty("data").EnumerateArray().Select(item => item.GetProperty("description").GetString()!).ToArray();

        (await AppointmentsAsync("dateFrom=2031-01-15&dateTo=2031-02-01")).ShouldBe(["January 20"], "from is inclusive, to is exclusive");
        (await AppointmentsAsync("dateFrom=2031-01-20")).ShouldBe(["January 20", "February 1"]);
        (await AppointmentsAsync("dateTo=2031-01-20T00:00:01Z")).ShouldBe(["January 10", "January 20"]);
        (await AppointmentsAsync($"dateFrom={Uri.EscapeDataString("2031-01-20T02:00:00+03:00")}&dateTo=2031-01-21")).ShouldBe(["January 20"], "offsets are normalized to UTC");

        var project = graph.Project.Id;
        await AddAssignmentsAsync(graph,
            ("Early", Utc(2031, 1, 1), Utc(2031, 1, 10)),
            ("Spanning", Utc(2031, 1, 5), Utc(2031, 2, 5)),
            ("Late", Utc(2031, 3, 1), Utc(2031, 3, 2)));

        async Task<string[]> AssignmentsAsync(string range) =>
            (await ListAsync($"assignments?projectId={project}&search=period&{range}&sortColumn=StartDate&pageSize=100"))
                .GetProperty("data").EnumerateArray().Select(item => item.GetProperty("name").GetString()!.Replace("period ", "")).ToArray();

        (await AssignmentsAsync("dateFrom=2031-01-20&dateTo=2031-02-01")).ShouldBe(["Spanning"], "periods overlapping the range match");
        (await AssignmentsAsync("dateFrom=2031-01-10")).ShouldBe(["Early", "Spanning", "Late"], "a period ending on dateFrom overlaps it");
        (await AssignmentsAsync("dateTo=2031-01-05")).ShouldBe(["Early"], "a period starting on dateTo does not");

        var grpc = await new ListAssignmentsCommand
        {
            Pagination = new PaginationParams { ProjectId = project.ToString(), Search = "period", DateFrom = "2031-01-20", DateTo = "2031-02-01" }
        }.RemoteExecuteAsync(WebAppFixture.GrpcOptions());
        grpc.Result.TotalCount.ShouldBe(1);
    }

    [Fact]
    public async Task Filters_NeverRevealRowsTheCallerCannotSee()
    {
        var graph = await app.CreateGraphAsync(WebAppFixture.Member);
        await AddAppointmentsAsync((graph.Assignment.Id, "Members only", DateTime.UtcNow));
        using var member = app.CreateClient(WebAppFixture.Member);
        using var outsider = app.CreateClient(WebAppFixture.Outsider);

        foreach (var query in new[]
                 {
                     $"assignments?projectId={graph.Project.Id}", $"appointments?assignmentId={graph.Assignment.Id}",
                     $"userProjects?projectId={graph.Project.Id}", $"projects?organizationId={graph.Organization.Id}"
                 })
        {
            (await ListAsync(query, member)).GetProperty("totalCount").GetInt32().ShouldBeGreaterThan(0, query);
            (await ListAsync(query, outsider)).GetProperty("totalCount").GetInt32().ShouldBe(0, query);
        }

        var grpc = await new ListAssignmentsCommand { Pagination = new PaginationParams { ProjectId = graph.Project.Id.ToString() } }
            .RemoteExecuteAsync(WebAppFixture.GrpcOptions(WebAppFixture.Outsider));
        grpc.Result.TotalCount.ShouldBe(0);
    }

    [Fact]
    public async Task LiveListings_ApplyTheSameFilters()
    {
        var mine = await app.CreateGraphAsync();
        var other = await app.CreateGraphAsync();
        await AddAppointmentsAsync((mine.Assignment.Id, "Streamed mine", DateTime.UtcNow), (other.Assignment.Id, "Streamed other", DateTime.UtcNow));
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(Cancellation);
        timeout.CancelAfter(TimeSpan.FromSeconds(10));
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/appointments?assignmentId={mine.Assignment.Id}&search=Streamed");
        request.Headers.Accept.ParseAdd("text/event-stream");

        using var response = await app.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, timeout.Token);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        using var reader = new StreamReader(await response.Content.ReadAsStreamAsync(timeout.Token));
        string? line;
        while ((line = await reader.ReadLineAsync(timeout.Token)) is not null && !line.StartsWith("data:", StringComparison.Ordinal)) { }
        line.ShouldNotBeNull();
        line.ShouldContain("Streamed mine");
        line.ShouldNotContain("Streamed other");
        line.ShouldContain("\"totalCount\":1");
    }

    private static DateTime Utc(int year, int month, int day) => new(year, month, day, 0, 0, 0, DateTimeKind.Utc);

    private async Task<JsonElement> ListAsync(string query, HttpClient? client = null)
    {
        var response = await (client ?? app.Client).GetAsync($"/api/{query}", Cancellation);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation);
        response.StatusCode.ShouldBe(HttpStatusCode.OK, body.ToString());
        return body.GetProperty("result");
    }

    private async Task AddAppointmentsAsync(params (Guid AssignmentId, string Description, DateTime KeepDate)[] rows)
    {
        await using var db = app.CreateDbContext();
        foreach (var (assignmentId, description, keepDate) in rows)
        {
            var userId = await db.Assignments!.Where(a => a.Id == assignmentId).Select(a => a.UserId).SingleAsync(Cancellation);
            db.Add(Domain.Entities.Appointment.Create(description, keepDate, 1, assignmentId, userId));
        }

        await db.SaveChangesAsync(Cancellation);
    }

    private async Task AddAssignmentsAsync(Graph graph, params (string Name, DateTime Start, DateTime End)[] periods)
    {
        await using var db = app.CreateDbContext();
        foreach (var (name, start, end) in periods)
            db.Add(Domain.Entities.Assignment.Create($"period {name}", "Dated", start, end, 1, graph.Project.Id, graph.Workflow.Id, graph.User.Id, graph.Type.Id));
        await db.SaveChangesAsync(Cancellation);
    }
}
