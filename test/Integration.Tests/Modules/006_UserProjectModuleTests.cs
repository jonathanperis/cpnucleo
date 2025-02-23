namespace Integration.Tests.Modules;

[Collection("Scenarios"), Order(6)]
public class UserProjectModuleTests(WebAppFixture fixture)
{
    private readonly IAlbaHost _host = fixture.AlbaHost;
    private readonly Guid _userProjectId = fixture.UserProjectId;
    private readonly Guid _userId = fixture.UserId;
    private readonly Guid _projectId = fixture.ProjectId;
    
    [Fact, Order(26)]
    public async Task UserProjects_ShouldCreateAnUserProject()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Post.Json(new CreateUserProjectCommand(_userId, _projectId, _userProjectId)).ToUrl("/api/userProjects");
            s.StatusCodeShouldBe(201);
        });
    }    
    
    [Fact, Order(27)]
    public async Task UserProjects_ShouldReturnAllUserProjects()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC"}).ToUrl("/api/userProjects");
            s.StatusCodeShouldBeOk();
        });
    }    
    
    [Fact, Order(28)]
    public async Task UserProjects_ShouldGetAnUserProject()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new{ }).ToUrl("/api/userProjects/" + _userProjectId);
            s.StatusCodeShouldBe(200);
        });      
    }
    
    [Fact, Order(29)]
    public async Task UserProjects_ShouldUpdateAnUserProject()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Patch.Json(new UpdateUserProjectCommand(_userProjectId, _userId, _projectId)).ToUrl("/api/userProjects/" + _userProjectId);
            s.StatusCodeShouldBe(204);
        });
    }
    
    [Fact, Order(30)]
    public async Task UserProjects_ShouldDeleteAnUserProject()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Delete.Json(new{ }).ToUrl("/api/userProjects/" + _userProjectId);
            s.StatusCodeShouldBe(204);
        });    
    }
}