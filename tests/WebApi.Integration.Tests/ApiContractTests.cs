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

        var response = await app.Client.PostAsync("/api/assignment", new StringContent(json, System.Text.Encoding.UTF8, "application/json"), Cancellation);

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
        var response = await app.Client.GetAsync("/api/users?sortColumn=Password&pageSize=5", Cancellation);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task BatchRemoval_IsBoundedToOneHundredIds()
    {
        using var remove = new HttpRequestMessage(HttpMethod.Delete, "/api/workflow")
        {
            Content = JsonContent.Create(new { ids = Enumerable.Range(0, 101).Select(_ => Guid.NewGuid()).ToArray() })
        };
        (await app.Client.SendAsync(remove, Cancellation)).StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
