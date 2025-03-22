namespace Integration.Tests.Modules;

[Collection("Scenarios"), Order(4)]
public class ImpedimentModuleTests(WebAppFixture fixture)
{
    private readonly IAlbaHost _host = fixture.AlbaHost;
    private readonly Guid _impedimentId = fixture.ImpedimentId;
    
    [Fact, Order(16)]
    public async Task Impediments_ShouldCreateAnImpediment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Post.Json(new CreateImpedimentCommand(
                $"Integration Test Impediment #{DateTime.UtcNow.Ticks.ToString()}", 
                _impedimentId)).ToUrl("/api/impediments");
            s.StatusCodeShouldBe(201);
        });
    }    
    
    [Fact, Order(17)]
    public async Task Impediments_ShouldReturnAllImpediments()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC"}).ToUrl("/api/impediments");
            s.StatusCodeShouldBeOk();
        });
    }    
    
    [Fact, Order(18)]
    public async Task Impediments_ShouldGetAnImpediment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new{ }).ToUrl("/api/impediments/" + _impedimentId);
            s.StatusCodeShouldBe(200);
        });      
    }
    
    [Fact, Order(19)]
    public async Task Impediments_ShouldUpdateAnImpediment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Patch.Json(new UpdateImpedimentCommand(_impedimentId,
                $"Integration Test Impediment UPDATED #{DateTime.UtcNow.Ticks.ToString()}")).ToUrl("/api/impediments/" + _impedimentId);
            s.StatusCodeShouldBe(204);
        });
    }
    
    [Fact, Order(20)]
    public async Task Impediments_ShouldDeleteAnImpediment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Delete.Json(new{ }).ToUrl("/api/impediments/" + _impedimentId);
            s.StatusCodeShouldBe(204);
        });    
    }
}