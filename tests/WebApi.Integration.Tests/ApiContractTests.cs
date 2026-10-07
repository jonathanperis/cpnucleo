using System.Net.Http.Json;
using System.Text.Json;
using Domain.Entities;

namespace WebApi.Integration.Tests;

/// <summary>Cross-cutting HTTP contract: error envelope, health endpoints, dates, search and sorting.</summary>
[Collection("Database")]
public class ApiContractTests(WebAppFixture app)
{
    private static CancellationToken Cancellation => TestContext.Current.CancellationToken;

    [Fact]
    public async Task Errors_UseOneEnvelope()
    {
        var missing = await app.Client.GetAsync($"/api/project?id={Guid.NewGuid()}", Cancellation);
        missing.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        var notFound = await missing.Content.ReadFromJsonAsync<JsonElement>(Cancellation);
        notFound.GetProperty("statusCode").GetInt32().ShouldBe(404);
        notFound.GetProperty("message").GetString().ShouldNotBeNullOrWhiteSpace();

        var invalid = await app.Client.PostAsJsonAsync("/api/workflow", new { id = Guid.NewGuid(), name = "", order = 0 }, Cancellation);
        invalid.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var validation = await invalid.Content.ReadFromJsonAsync<JsonElement>(Cancellation);
        validation.GetProperty("statusCode").GetInt32().ShouldBe(400);
        validation.GetProperty("errors").TryGetProperty("name", out _).ShouldBeTrue(validation.ToString());
        validation.GetProperty("errors").TryGetProperty("order", out _).ShouldBeTrue(validation.ToString());
    }

    [Fact]
    public async Task DomainRules_ReportTheOffendingField()
    {
        var graph = await app.CreateGraphAsync();
        var response = await app.Client.PostAsJsonAsync("/api/assignment", new
        {
            id = Guid.NewGuid(), name = "Backwards", description = "x", startDate = DateTime.UtcNow, endDate = DateTime.UtcNow.AddDays(-1),
            amountHours = 1, projectId = graph.Project.Id, workflowId = graph.Workflow.Id, userId = graph.User.Id, assignmentTypeId = graph.Type.Id
        }, Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation);
        body.GetProperty("errors").GetProperty("endDate")[0].GetString().ShouldBe("End date must be on or after start date.");
    }

    [Theory]
    [InlineData("2064-06-09")]
    [InlineData("2064-06-09T10:00:00")]
    [InlineData("2064-06-09T10:00:00+03:00")]
    public async Task Dates_WithoutUtcDesignator_AreAcceptedAsUtc(string date)
    {
        var graph = await app.CreateGraphAsync();
        var id = Guid.NewGuid();
        var json = $$"""
            {"id":"{{id}}","name":"Dates","description":"x","startDate":"{{date}}","endDate":"{{date}}","amountHours":1,
             "projectId":"{{graph.Project.Id}}","workflowId":"{{graph.Workflow.Id}}","userId":"{{graph.User.Id}}","assignmentTypeId":"{{graph.Type.Id}}"}
            """;

        using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await app.Client.PostAsync("/api/assignment", content, Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.OK, await response.Content.ReadAsStringAsync(Cancellation));
    }

    [Fact]
    public async Task ApiResponses_CarrySecurityHeaders()
    {
        var response = await app.Client.GetAsync("/api/organizations?pageSize=1", Cancellation);

        response.Headers.GetValues("Strict-Transport-Security").Single().ShouldContain("max-age=");
        response.Headers.GetValues("X-Content-Type-Options").Single().ShouldBe("nosniff");
        response.Headers.GetValues("X-Frame-Options").Single().ShouldBe("DENY");
        response.Headers.GetValues("Content-Security-Policy").Single().ShouldContain("default-src 'none'");
    }

    [Fact]
    public async Task HealthEndpoints_ReportLivenessAndSchemaReadiness()
    {
        using var anonymous = app.CreateClient();
        (await anonymous.GetAsync("/healthz", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await anonymous.GetAsync("/readyz", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Search_TreatsWildcardsLiterally()
    {
        var graph = await app.CreateGraphAsync();
        var marker = Guid.NewGuid().ToString("N")[..8];
        var literal = Project.Create($"{marker} 100% done", graph.Organization.Id);
        var lookalike = Project.Create($"{marker} 1000 done", graph.Organization.Id);
        await using (var db = app.CreateDbContext())
        {
            db.AddRange(literal, lookalike);
            await db.SaveChangesAsync(Cancellation);
        }

        var result = await app.Client.GetFromJsonAsync<JsonElement>($"/api/projects?search={Uri.EscapeDataString(marker + " 100%")}", Cancellation);

        result.GetProperty("result").GetProperty("totalCount").GetInt32().ShouldBe(1);
        result.GetProperty("result").GetProperty("data")[0].GetProperty("id").GetGuid().ShouldBe(literal.Id);
    }

    [Theory]
    [InlineData("ASC")]
    [InlineData("DESC")]
    public async Task Sorting_OrdersByTheCanonicalColumn(string order)
    {
        var first = Organization.Create($"A {Guid.NewGuid():N}", null);
        var second = Organization.Create($"B {Guid.NewGuid():N}", null);
        await using (var db = app.CreateDbContext())
        {
            db.AddRange(second, first);
            await db.SaveChangesAsync(Cancellation);
        }

        var result = await app.Client.GetFromJsonAsync<JsonElement>(
            $"/api/organizations?ids={first.Id},{second.Id}&sortColumn=name&sortOrder={order}", Cancellation);

        var names = result.GetProperty("result").GetProperty("data").EnumerateArray().Select(item => item.GetProperty("name").GetString()).ToArray();
        names.ShouldBe(order == "ASC" ? [first.Name, second.Name] : [second.Name, first.Name]);
    }

    [Fact]
    public async Task Users_CannotBeSortedByCredentials()
    {
        var ids = string.Join(',', new[] { WebAppFixture.Admin, WebAppFixture.Member, WebAppFixture.Outsider }.Select(a => a.Id));
        var byPassword = await app.Client.GetFromJsonAsync<JsonElement>($"/api/users?ids={ids}&sortColumn=Password&sortOrder=DESC", Cancellation);
        var byId = await app.Client.GetFromJsonAsync<JsonElement>($"/api/users?ids={ids}&sortColumn=Id&sortOrder=DESC", Cancellation);

        Ids(byPassword).ShouldBe(Ids(byId), "an unsortable column falls back to Id instead of ordering by password hashes");
        static Guid[] Ids(JsonElement page) => page.GetProperty("result").GetProperty("data").EnumerateArray().Select(u => u.GetProperty("id").GetGuid()).ToArray();
    }

    [Theory]
    [InlineData("impediments")]
    [InlineData("workflows")]
    public async Task Search_TreatsUnderscoreLiterally_OnEfAndGenericDapperLists(string plural)
    {
        var marker = Guid.NewGuid().ToString("N")[..8];
        BaseEntity literal = plural == "impediments" ? Impediment.Create($"{marker} a_b") : Workflow.Create($"{marker} a_b", 1);
        BaseEntity lookalike = plural == "impediments" ? Impediment.Create($"{marker} axb") : Workflow.Create($"{marker} axb", 2);
        await using (var db = app.CreateDbContext())
        {
            db.AddRange(literal, lookalike);
            await db.SaveChangesAsync(Cancellation);
        }

        var result = await app.Client.GetFromJsonAsync<JsonElement>($"/api/{plural}?search={Uri.EscapeDataString(marker + " a_b")}", Cancellation);

        result.GetProperty("result").GetProperty("totalCount").GetInt32().ShouldBe(1);
        result.GetProperty("result").GetProperty("data")[0].GetProperty("id").GetGuid().ShouldBe(literal.Id);
    }

    [Fact]
    public async Task ConditionalRequests_AreValidatedPerCaller()
    {
        var first = await app.Client.GetAsync("/api/organizations?pageSize=1", Cancellation);
        var etag = first.Headers.ETag;
        etag.ShouldNotBeNull();

        using var member = app.CreateClient(WebAppFixture.Member);
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/organizations?pageSize=1");
        request.Headers.IfNoneMatch.Add(etag);
        (await member.SendAsync(request, Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK,
            "another account's validator must not turn into a 304 for this caller's (differently filtered) data");

        using var same = new HttpRequestMessage(HttpMethod.Get, "/api/organizations?pageSize=1");
        same.Headers.IfNoneMatch.Add(etag);
        (await app.Client.SendAsync(same, Cancellation)).StatusCode.ShouldBe(HttpStatusCode.NotModified);
    }

    [Fact]
    public async Task ErrorResponses_KeepSecurityHeaders()
    {
        using var member = app.CreateClient(WebAppFixture.Member);
        var forbidden = await member.PostAsJsonAsync("/api/organization", new { id = Guid.NewGuid(), name = "x", description = "x" }, Cancellation);
        forbidden.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        forbidden.Headers.GetValues("X-Content-Type-Options").Single().ShouldBe("nosniff");
        forbidden.Headers.GetValues("Content-Security-Policy").Single().ShouldContain("default-src 'none'");

        using var identity = app.CreateIdentityClient();
        // A sign-in form post without the WebClient origin is refused with the error envelope.
        using var form = new FormUrlEncodedContent(new Dictionary<string, string> { ["login"] = "x" });
        var refused = await identity.PostAsync("/api/account/login", form, Cancellation);
        refused.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        refused.Headers.GetValues("X-Frame-Options").Single().ShouldBe("DENY");
        refused.Headers.GetValues("Strict-Transport-Security").Single().ShouldContain("max-age=");
    }

    [Fact]
    public async Task WebApiRateLimit_RejectsWithRetryAfterAndTheErrorEnvelope()
    {
        await using var limited = app.CreateRateLimitedWebApiFactory();
        using var client = limited.CreateClient();
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(Cancellation);
        timeout.CancelAfter(TimeSpan.FromSeconds(30));

        // Health probes are exempt, but every other request counts against the per-address window
        // (300 permits plus a queue of 20), including anonymous ones.
        var requests = Enumerable.Range(0, 330).Select(_ => client.GetAsync("/", timeout.Token)).ToList();
        HttpResponseMessage? rejected = null;
        while (requests.Count > 0 && rejected is null)
        {
            var completed = await Task.WhenAny(requests);
            requests.Remove(completed);
            var response = await completed;
            if (response.StatusCode == HttpStatusCode.TooManyRequests) rejected = response;
        }

        rejected.ShouldNotBeNull();
        rejected.Headers.RetryAfter.ShouldNotBeNull();
        (await rejected.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("statusCode").GetInt32().ShouldBe(429);
        await timeout.CancelAsync();
    }

    [Theory]
    [InlineData("workflow", 101)]
    [InlineData("project", 101)]
    [InlineData("project", 0)]
    [InlineData("organization", 0)]
    public async Task BatchRemoval_RequiresOneToOneHundredIds(string singular, int count)
    {
        using var remove = new HttpRequestMessage(HttpMethod.Delete, $"/api/{singular}")
        {
            Content = JsonContent.Create(new { ids = Enumerable.Range(0, count).Select(_ => Guid.NewGuid()).ToArray() })
        };
        var response = await app.Client.SendAsync(remove, Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("errors").TryGetProperty("ids", out _).ShouldBeTrue();
    }

    [Fact]
    public async Task GrpcBatchRemoval_IsBoundedToOneHundredIds()
    {
        var command = new GrpcServer.Contracts.Commands.Workflow.RemoveWorkflowCommand { Ids = Enumerable.Range(0, 101).Select(_ => Guid.NewGuid()).ToList() };
        (await Should.ThrowAsync<Grpc.Core.RpcException>(() => command.RemoteExecuteAsync(WebAppFixture.GrpcOptions())))
            .StatusCode.ShouldBe(Grpc.Core.StatusCode.InvalidArgument);
    }
}
