using System.Net.Http.Json;
using System.Text.Json;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer.Contracts.Commands.Workflow;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Integration.Tests;

/// <summary>
/// Invalid paging is a validation failure on both transports, whatever produced the request:
/// REST query binding, the typed gRPC client, or a hand-crafted gRPC message.
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
        { $"search={new string('x', 129)}", "search" }
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
}
