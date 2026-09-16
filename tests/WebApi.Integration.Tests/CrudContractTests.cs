using System.Net.Http.Json;
using System.Text.Json;
using Domain.Common.Security;
using Domain.Entities;

namespace WebApi.Integration.Tests;

[Collection("Database")]
public class CrudContractTests(WebAppFixture app)
{
    [Theory]
    [InlineData("organization", "organizations")]
    [InlineData("project", "projects")]
    [InlineData("user", "users")]
    [InlineData("workflow", "workflows")]
    [InlineData("impediment", "impediments")]
    [InlineData("assignmentType", "assignmentTypes")]
    [InlineData("userProject", "userProjects")]
    [InlineData("assignment", "assignments")]
    [InlineData("assignmentImpediment", "assignmentImpediments")]
    [InlineData("userAssignment", "userAssignments")]
    [InlineData("appointment", "appointments")]
    public async Task EachResource_CompletesAnIndependentCrudCycle(string singular, string plural)
    {
        var ct = TestContext.Current.CancellationToken;
        var organization = Organization.Create("Prerequisite", "Prerequisite");
        var project = Project.Create("Prerequisite", organization.Id);
        var user = User.Create("Prerequisite", $"test-{Guid.NewGuid():N}", new PasswordHash("unused-test-hash", ""));
        var workflow = Workflow.Create("Prerequisite", 1);
        var type = AssignmentType.Create("Prerequisite");
        var impediment = Impediment.Create("Prerequisite");
        var assignment = Assignment.Create("Prerequisite", "Prerequisite", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 1, project.Id, workflow.Id, user.Id, type.Id);
        await using (var db = app.CreateDbContext())
        {
            db.AddRange(organization, project, user, workflow, type, impediment, assignment);
            await db.SaveChangesAsync(ct);
        }

        var id = Guid.NewGuid();
        var payload = new Dictionary<string, object>
        {
            ["id"] = id, ["name"] = "Created", ["description"] = "Created", ["order"] = 1,
            ["login"] = $"crud-{id:N}", ["password"] = "Integration@123", ["amountHours"] = 2,
            ["startDate"] = DateTime.UtcNow, ["endDate"] = DateTime.UtcNow.AddDays(1), ["keepDate"] = DateTime.UtcNow,
            ["organizationId"] = organization.Id, ["projectId"] = project.Id, ["userId"] = user.Id,
            ["workflowId"] = workflow.Id, ["assignmentTypeId"] = type.Id, ["impedimentId"] = impediment.Id, ["assignmentId"] = assignment.Id
        };
        (await app.Client.PostAsJsonAsync($"/api/{singular}", payload, ct)).StatusCode.ShouldBe(HttpStatusCode.OK);
        var item = await app.Client.GetFromJsonAsync<JsonElement>($"/api/{singular}?id={id}", ct);
        item.GetProperty(singular).GetProperty("id").GetGuid().ShouldBe(id);
        (await app.Client.GetAsync($"/api/{plural}?pageSize=100", ct)).StatusCode.ShouldBe(HttpStatusCode.OK);
        payload["name"] = "Updated";
        payload["description"] = "Updated";
        (await app.Client.PatchAsJsonAsync($"/api/{singular}", payload, ct)).StatusCode.ShouldBe(HttpStatusCode.OK);
        var updated = await app.Client.GetFromJsonAsync<JsonElement>($"/api/{singular}?id={id}", ct);
        if (updated.GetProperty(singular).TryGetProperty("name", out var name)) name.GetString().ShouldBe("Updated");
        using var remove = new HttpRequestMessage(HttpMethod.Delete, $"/api/{singular}") { Content = JsonContent.Create(new { ids = new[] { id } }) };
        (await app.Client.SendAsync(remove, ct)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await app.Client.GetAsync($"/api/{singular}?id={id}", ct)).StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
