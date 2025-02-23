namespace Integration.Tests.Modules;

[Collection("Scenarios"), Order(3)]
public class WorkflowModuleTests(WebAppFixture fixture)
{
    private readonly IAlbaHost _host = fixture.AlbaHost;
    private readonly Guid _workflowId = fixture.WorkflowId;

    [Fact, Order(11)]
    public async Task Workflows_ShouldCreateAnWorkflow()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Post.Json(new CreateWorkflowCommand(
                $"Integration Test Workflow #{DateTime.UtcNow.Ticks.ToString()}", 
                1, 
                _workflowId)).ToUrl("/api/workflows");
            s.StatusCodeShouldBe(201);
        });
    }    
    
    [Fact, Order(12)]
    public async Task Workflows_ShouldReturnAllWorkflows()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC"}).ToUrl("/api/workflows");
            s.StatusCodeShouldBeOk();
        });
    }    
    
    [Fact, Order(13)]
    public async Task Workflows_ShouldGetAnWorkflow()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new{ }).ToUrl("/api/workflows/" + _workflowId);
            s.StatusCodeShouldBe(200);
        });      
    }
    
    [Fact, Order(14)]
    public async Task Workflows_ShouldUpdateAnWorkflow()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Patch.Json(new UpdateWorkflowCommand(_workflowId,
                $"Integration Test Workflow UPDATED #{DateTime.UtcNow.Ticks.ToString()}", 
            2)).ToUrl("/api/workflows/" + _workflowId);
            s.StatusCodeShouldBe(204);
        });
    }
    
    [Fact, Order(15)]
    public async Task Workflows_ShouldDeleteAnWorkflow()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Delete.Json(new{ }).ToUrl("/api/workflows/" + _workflowId);
            s.StatusCodeShouldBe(204);
        });    
    }
}