namespace Integration.Tests.Modules;

[Collection("Scenarios"), Order(7)]
public class AssignmentTypeModuleTests(WebAppFixture fixture)
{    
    private readonly IAlbaHost _host = fixture.AlbaHost;
    private readonly Guid _assignmentTypeId = fixture.AssignmentTypeId;
    
    [Fact, Order(31)]
    public async Task AssignmentTypes_ShouldCreateAnAssignmentType()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Post.Json(new CreateAssignmentTypeCommand(
                $"Integration Test AssignmentType #{DateTime.UtcNow.Ticks.ToString()}",
                _assignmentTypeId)).ToUrl("/api/assignmentTypes");
            s.StatusCodeShouldBe(201);
        });
    }    
    
    [Fact, Order(32)]
    public async Task AssignmentTypes_ShouldReturnAllAssignmentTypes()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC"}).ToUrl("/api/assignmentTypes");
            s.StatusCodeShouldBeOk();
        });
    }    
    
    [Fact, Order(33)]
    public async Task AssignmentTypes_ShouldGetAnAssignmentType()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new{ }).ToUrl("/api/assignmentTypes/" + _assignmentTypeId);
            s.StatusCodeShouldBe(200);
        });      
    }
    
    [Fact, Order(34)]
    public async Task AssignmentTypes_ShouldUpdateAnAssignmentType()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Patch.Json(new UpdateAssignmentTypeCommand(_assignmentTypeId,
                $"Integration Test AssignmentType UPDATED #{DateTime.UtcNow.Ticks.ToString()}")).ToUrl("/api/assignmentTypes/" + _assignmentTypeId);
            s.StatusCodeShouldBe(204);
        });
    }
    
    [Fact, Order(35)]
    public async Task AssignmentTypes_ShouldDeleteAnAssignmentType()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Delete.Json(new{ }).ToUrl("/api/assignmentTypes/" + _assignmentTypeId);
            s.StatusCodeShouldBe(204);
        });    
    }
}