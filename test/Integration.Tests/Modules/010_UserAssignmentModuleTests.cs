namespace Integration.Tests.Modules;

[Collection("Scenarios"), Order(10)]
public class UserAssignmentModuleTests(WebAppFixture fixture)
{
    private readonly IAlbaHost _host = fixture.AlbaHost;
    private readonly Guid _userAssignmentId = fixture.UserAssignmentId;
    private readonly Guid _userId = fixture.UserId;
    private readonly Guid _assignmentId = fixture.AssignmentId;
    
    [Fact, Order(46)]
    public async Task UserAssignments_ShouldCreateAnUserAssignment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Post.Json(new CreateUserAssignmentCommand(_userId, _assignmentId, _userAssignmentId)).ToUrl("/api/userAssignments");
            s.StatusCodeShouldBe(201);
        });
    }    
    
    [Fact, Order(47)]
    public async Task UserAssignments_ShouldReturnAllUserAssignments()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC"}).ToUrl("/api/userAssignments");
            s.StatusCodeShouldBeOk();
        });
    }    
    
    [Fact, Order(48)]
    public async Task UserAssignments_ShouldGetAnUserAssignment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new{ }).ToUrl("/api/userAssignments/" + _userAssignmentId);
            s.StatusCodeShouldBe(200);
        });      
    }
    
    [Fact, Order(49)]
    public async Task UserAssignments_ShouldUpdateAnUserAssignment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Patch.Json(new UpdateUserAssignmentCommand(_userAssignmentId, _userId, _assignmentId)).ToUrl("/api/userAssignments/" + _userAssignmentId);
            s.StatusCodeShouldBe(204);
        });
    }
    
    [Fact, Order(50)]
    public async Task UserAssignments_ShouldDeleteAnUserAssignment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Delete.Json(new{ }).ToUrl("/api/userAssignments/" + _userAssignmentId);
            s.StatusCodeShouldBe(204);
        });    
    }
}