namespace Integration.Tests.Modules;

[Collection("Scenarios"), Order(8)]
public class AssignmentModuleTests(WebAppFixture fixture)
{
    private readonly IAlbaHost _host = fixture.AlbaHost;
    private readonly Guid _assignmentId = fixture.AssignmentId;
    private readonly Guid _projectId = fixture.ProjectId;
    private readonly Guid _workflowId = fixture.WorkflowId;
    private readonly Guid _userId = fixture.UserId;
    private readonly Guid _assignmentTypeId = fixture.AssignmentTypeId;
    
    [Fact, Order(36)]
    public async Task Assignments_ShouldCreateAnAssignment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Post.Json(new CreateAssignmentCommand(
                $"Integration Test Assignment #{DateTime.UtcNow.Ticks.ToString()}", 
                "Integration Test Assignment Description",
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(7),
                30,
                _projectId,
                _workflowId,
                _userId,
                _assignmentTypeId,
                _assignmentId)).ToUrl("/api/assignments");
            s.StatusCodeShouldBe(201);
        });
    }    
    
    [Fact, Order(37)]
    public async Task Assignments_ShouldReturnAllAssignments()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC"}).ToUrl("/api/assignments");
            s.StatusCodeShouldBeOk();
        });
    }    
    
    [Fact, Order(38)]
    public async Task Assignments_ShouldGetAnAssignment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new{ }).ToUrl("/api/assignments/" + _assignmentId);
            s.StatusCodeShouldBe(200);
        });      
    }
    
    [Fact, Order(39)]
    public async Task Assignments_ShouldUpdateAnAssignment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Patch.Json(new UpdateAssignmentCommand(_assignmentId,
                $"Integration Test Assignment #{DateTime.UtcNow.Ticks.ToString()}",
                "Integration Test Assignment Description",
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(7),
                30,
                _projectId,
                _workflowId,
                _userId,
                _assignmentTypeId)).ToUrl("/api/assignments/" + _assignmentId);
            s.StatusCodeShouldBe(204);
        });
    }
    
    [Fact, Order(40)]
    public async Task Assignments_ShouldDeleteAnAssignment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Delete.Json(new{ }).ToUrl("/api/assignments/" + _assignmentId);
            s.StatusCodeShouldBe(204);
        });    
    }
}