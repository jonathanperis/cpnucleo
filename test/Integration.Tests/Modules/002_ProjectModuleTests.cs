namespace Integration.Tests.Modules;

[Collection("Scenarios"), Order(2)]
public class ProjectModuleTests(WebAppFixture fixture)
{
    private readonly IAlbaHost _host = fixture.AlbaHost;
    private readonly Guid _organizationId = fixture.OrganizationId;
    private readonly Guid _projectId = fixture.ProjectId;
    
    [Fact, Order(6)]
    public async Task Projects_ShouldCreateAnProject()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Post.Json(new CreateProjectCommand($"Integration Test Project #{DateTime.UtcNow.Ticks.ToString()}", 
                _organizationId, 
                _projectId)).ToUrl("/api/projects");
            s.StatusCodeShouldBe(201);
        });
    }    
    
    [Fact, Order(7)]
    public async Task Projects_ShouldReturnAllProjects()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC"}).ToUrl("/api/projects");
            s.StatusCodeShouldBeOk();
        });
    }    
    
    [Fact, Order(8)]
    public async Task Projects_ShouldGetAnProject()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new{ }).ToUrl("/api/projects/" + _projectId);
            s.StatusCodeShouldBe(200);
        });      
    }
    
    [Fact, Order(9)]
    public async Task Projects_ShouldUpdateAnProject()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Patch.Json(new UpdateProjectCommand(_projectId, 
                $"Integration Test Project UPDATED #{DateTime.UtcNow.Ticks.ToString()}", 
                _organizationId)).ToUrl("/api/projects/" + _projectId);
            s.StatusCodeShouldBe(204);
        });
    }
    
    [Fact, Order(10)]
    public async Task Projects_ShouldDeleteAnProject()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Delete.Json(new{ }).ToUrl("/api/projects/" + _projectId);
            s.StatusCodeShouldBe(204);
        });    
    }
}