using System.Text.Json;
using Domain.Common;
using Infrastructure.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace WebApi.Unit.Tests.Common.Http;

public class ApiExceptionMiddlewareTests
{
    [Test]
    public async Task UnexpectedErrors_ReturnAGenericEnvelopeWithoutDetails()
    {
        var (status, body) = await InvokeAsync(new InvalidOperationException("connection string Password=secret-value"));

        status.ShouldBe(500);
        body.GetProperty("statusCode").GetInt32().ShouldBe(500);
        body.GetProperty("message").GetString().ShouldBe("An unexpected error occurred.");
        body.ToString().ShouldNotContain("secret-value");
    }

    [Test]
    public async Task InternalArgumentErrors_DoNotLeakTheirMessage()
    {
        var (status, body) = await InvokeAsync(new ArgumentException("internal parameter detail"));

        status.ShouldBe(400);
        body.ToString().ShouldNotContain("internal parameter detail");
    }

    [Test]
    public async Task DomainRules_ReportAClientSafeMessageKeyedByField()
    {
        var (status, body) = await InvokeAsync(new DomainException("AmountHours must be greater than 0.", "AmountHours"));

        status.ShouldBe(400);
        body.GetProperty("errors").GetProperty("amountHours")[0].GetString().ShouldBe("AmountHours must be greater than 0.");
    }

    [Test]
    public async Task HiddenAndDeniedRecords_MapTo404And403()
    {
        (await InvokeAsync(new RecordNotFoundException())).Status.ShouldBe(404);
        (await InvokeAsync(new AccessDeniedException("You are not a member of the project."))).Status.ShouldBe(403);
    }

    [TestCase("23503", 400)]
    [TestCase("23001", 409)]
    [TestCase("23505", 409)]
    [TestCase("40P01", 409)]
    public async Task DatabaseIntegrityErrors_MapToClientSafeStatuses(string sqlState, int expected)
    {
        var exception = new Npgsql.PostgresException("internal trigger text", "ERROR", "ERROR", sqlState, columnName: "ProjectId", tableName: "Assignments");

        var (status, body) = await InvokeAsync(exception);

        status.ShouldBe(expected);
        body.ToString().ShouldNotContain("internal trigger text");
    }

    private static async Task<(int Status, JsonElement Body)> InvokeAsync(Exception exception)
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var middleware = new ApiExceptionMiddleware(_ => throw exception, NullLogger<ApiExceptionMiddleware>.Instance);

        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;
        return (context.Response.StatusCode, await JsonSerializer.DeserializeAsync<JsonElement>(context.Response.Body));
    }
}
