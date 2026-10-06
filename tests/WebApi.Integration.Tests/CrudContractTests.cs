using System.Net.Http.Json;
using System.Text.Json;
using Domain.Common.Security;
using Domain.Entities;

namespace WebApi.Integration.Tests;

[Collection("Database")]
public class CrudContractTests(WebAppFixture app)
{
    [Theory]
    [InlineData("organization", "organizations", "Organizations")]
    [InlineData("project", "projects", "Projects")]
    [InlineData("user", "users", "Users")]
    [InlineData("workflow", "workflows", "Workflows")]
    [InlineData("impediment", "impediments", "Impediments")]
    [InlineData("assignmentType", "assignmentTypes", "AssignmentTypes")]
    [InlineData("userProject", "userProjects", "UserProjects")]
    [InlineData("assignment", "assignments", "Assignments")]
    [InlineData("assignmentImpediment", "assignmentImpediments", "AssignmentImpediments")]
    [InlineData("userAssignment", "userAssignments", "UserAssignments")]
    [InlineData("appointment", "appointments", "Appointments")]
    public async Task EachResource_CompletesAnIndependentCrudCycle(string singular, string plural, string table)
    {
        var ct = TestContext.Current.CancellationToken;
        var graph = await app.CreateGraphAsync();
        var secondUser = User.Create("Second", $"second-{Guid.NewGuid():N}", new PasswordHash("unused-test-hash", ""));
        await using (var db = app.CreateDbContext())
        {
            db.Add(secondUser);
            await db.SaveChangesAsync(ct);
        }

        var id = Guid.NewGuid();
        var payload = new Dictionary<string, object>
        {
            ["id"] = id, ["name"] = "Created", ["description"] = "Created", ["order"] = 1,
            ["login"] = $"crud-{id:N}", ["password"] = "Integration@123", ["amountHours"] = 2,
            ["startDate"] = DateTime.UtcNow, ["endDate"] = DateTime.UtcNow.AddDays(1), ["keepDate"] = DateTime.UtcNow,
            ["organizationId"] = graph.Organization.Id, ["projectId"] = graph.Project.Id, ["userId"] = graph.User.Id,
            ["workflowId"] = graph.Workflow.Id, ["assignmentTypeId"] = graph.Type.Id, ["impedimentId"] = graph.Impediment.Id,
            ["assignmentId"] = graph.Assignment.Id
        };
        (await app.Client.PostAsJsonAsync($"/api/{singular}", payload, ct)).StatusCode.ShouldBe(HttpStatusCode.OK);
        var item = await app.Client.GetFromJsonAsync<JsonElement>($"/api/{singular}?id={id}", ct);
        item.GetProperty(singular).GetProperty("id").GetGuid().ShouldBe(id);
        var list = await app.Client.GetFromJsonAsync<JsonElement>($"/api/{plural}?ids={id}&pageSize=100", ct);
        list.GetProperty("result").GetProperty("totalCount").GetInt32().ShouldBe(1);

        payload["name"] = "Updated";
        payload["description"] = "Updated";
        payload["order"] = 2;
        payload["amountHours"] = 3;
        payload["userId"] = secondUser.Id;
        (await app.Client.PatchAsJsonAsync($"/api/{singular}", payload, ct)).StatusCode.ShouldBe(HttpStatusCode.OK);
        var updated = (await app.Client.GetFromJsonAsync<JsonElement>($"/api/{singular}?id={id}", ct)).GetProperty(singular);
        ExpectedChanges(singular, secondUser.Id).ShouldAllBe(change => change.Matches(updated), $"{singular} must persist its update");

        using var remove = new HttpRequestMessage(HttpMethod.Delete, $"/api/{singular}") { Content = JsonContent.Create(new { ids = new[] { id, id } }) };
        (await app.Client.SendAsync(remove, ct)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await app.Client.GetAsync($"/api/{singular}?id={id}", ct)).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        (await app.RowStateAsync(table, id)).ShouldBe((false, true), "removal is a soft delete: the row stays with Active=false and DeletedAt set");
    }

    private sealed record Change(string Property, object Expected)
    {
        public bool Matches(JsonElement item) => Expected switch
        {
            Guid guid => item.GetProperty(Property).GetGuid() == guid,
            int number => item.GetProperty(Property).GetInt32() == number,
            _ => item.GetProperty(Property).GetString() == (string)Expected
        };
    }

    private static Change[] ExpectedChanges(string singular, Guid secondUserId) => singular switch
    {
        "organization" => [new("name", "Updated"), new("description", "Updated")],
        "workflow" => [new("name", "Updated"), new("order", 2)],
        "userProject" or "userAssignment" => [new("userId", secondUserId)],
        "assignment" => [new("name", "Updated"), new("description", "Updated"), new("amountHours", 3), new("userId", secondUserId)],
        "assignmentImpediment" => [new("description", "Updated")],
        "appointment" => [new("description", "Updated"), new("amountHours", 3), new("userId", secondUserId)],
        _ => [new("name", "Updated")]
    };
}
