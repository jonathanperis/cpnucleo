namespace Integration.Tests.Modules;

[Collection("Scenarios"), Order(9)]
public class AssignmentImpedimentModuleTests(WebAppFixture fixture)
{
    private readonly IAlbaHost _host = fixture.AlbaHost;
    private readonly Guid _assignmentImpedimentId = fixture.AssignmentImpedimentId;
    private readonly Guid _assignmentId = fixture.AssignmentId;
    private readonly Guid _impedimentId = fixture.ImpedimentId;

    [Fact, Order(41)]
    public async Task AssignmentImpediments_ShouldCreateAnAssignmentImpediment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Post.Json(new CreateAssignmentImpedimentCommand(
                $"Integration Test AssignmentImpediment #{DateTime.UtcNow.Ticks.ToString()}", 
                _assignmentId,
                _impedimentId,
                _assignmentImpedimentId)).ToUrl("/api/assignmentImpediments");
            s.StatusCodeShouldBe(201);
        });
    }    
    
    [Fact, Order(42)]
    public async Task AssignmentImpediments_ShouldReturnAllAssignmentImpediments()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC"}).ToUrl("/api/assignmentImpediments");
            s.StatusCodeShouldBeOk();
        });
    }    
    
    [Fact, Order(43)]
    public async Task AssignmentImpediments_ShouldGetAnAssignmentImpediment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new{ }).ToUrl("/api/assignmentImpediments/" + _assignmentImpedimentId);
            s.StatusCodeShouldBe(200);
        });      
    }
    
    [Fact, Order(44)]
    public async Task AssignmentImpediments_ShouldUpdateAnAssignmentImpediment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Patch.Json(new UpdateAssignmentImpedimentCommand(_assignmentImpedimentId,
                $"Integration Test AssignmentImpediment UPDATED #{DateTime.UtcNow.Ticks.ToString()}", 
                _assignmentId,
                _impedimentId)).ToUrl("/api/assignmentImpediments/" + _assignmentImpedimentId);
            s.StatusCodeShouldBe(204);
        });
    }
    
    [Fact, Order(45)]
    public async Task AssignmentImpediments_ShouldDeleteAnAssignmentImpediment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Delete.Json(new{ }).ToUrl("/api/assignmentImpediments/" + _assignmentImpedimentId);
            s.StatusCodeShouldBe(204);
        });    
    }
}