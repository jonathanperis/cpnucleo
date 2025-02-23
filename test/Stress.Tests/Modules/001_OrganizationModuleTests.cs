namespace Stress.Tests.Modules;

public class OrganizationModuleTests()
{
    Counter _testCounter;

    private IAlbaHost Host { get; set; }

    [PerfSetup]
    public void Setup(BenchmarkContext context)
    {
        var securityStub = new JwtSecurityStub();
        Host = AlbaHost.For<WebApi.Program2>(securityStub).GetAwaiter().GetResult();
        
        _testCounter = context.GetCounter("Test");
    }

    [CounterThroughputAssertion("Test", MustBe.GreaterThan, 100)]
    [PerfBenchmark(NumberOfIterations = 5, RunMode = RunMode.Throughput, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
    public void Organizations_ShouldCreateAnOrganization()
    {        
        // Arrange //Act // Assert
        Host.Scenario(s =>
        {
            s.Get.Json(new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC"}).ToUrl("/api/organizations");
            s.StatusCodeShouldBeOk();
        });
    }
    
    // private readonly IAlbaHost _host = fixture.AlbaHost;
    // private readonly Guid _organizationId = fixture.OrganizationId;
    //
    // [Fact, Order(1)]
    // public async Task Organizations_ShouldCreateAnOrganization()
    // {
    //     // Arrange //Act // Assert
    //     await _host.Scenario(s =>
    //     {
    //         s.Post.Json(new CreateOrganizationCommand(
    //             $"Integration Test Organization #{DateTime.UtcNow.Ticks.ToString()}", 
    //             "Integration Test Organization Description", 
    //             _organizationId)).ToUrl("/api/organizations");
    //         s.StatusCodeShouldBe(201);
    //     });
    // }    
    //
    // [Fact, Order(2)]
    // public async Task Organizations_ShouldReturnAllOrganizations()
    // {
    //     // Arrange //Act // Assert
    //     await _host.Scenario(s =>
    //     {
    //         s.Get.Json(new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC"}).ToUrl("/api/organizations");
    //         s.StatusCodeShouldBeOk();
    //     });
    // }    
    //
    // [Fact, Order(3)]
    // public async Task Organizations_ShouldGetAnOrganization()
    // {
    //     // Arrange //Act // Assert
    //     await _host.Scenario(s =>
    //     {
    //         s.Get.Json(new{ }).ToUrl("/api/organizations/" + _organizationId);
    //         s.StatusCodeShouldBe(200);
    //     });      
    // }
    //
    // [Fact, Order(4)]
    // public async Task Organizations_ShouldUpdateAnOrganization()
    // {
    //     // Arrange //Act // Assert
    //     await _host.Scenario(s =>
    //     {
    //         s.Patch.Json(new UpdateOrganizationCommand(_organizationId,
    //             $"Integration Test Organization UPDATED #{DateTime.UtcNow.Ticks.ToString()}", 
    //         "Integration Test Organization Description UPDATED")).ToUrl("/api/organizations/" + _organizationId);
    //         s.StatusCodeShouldBe(204);
    //     });
    // }
    //
    // [Fact, Order(5)]
    // public async Task Organizations_ShouldDeleteAnOrganization()
    // {
    //     // Arrange //Act // Assert
    //     await _host.Scenario(s =>
    //     {
    //         s.Delete.Json(new{ }).ToUrl("/api/organizations/" + _organizationId);
    //         s.StatusCodeShouldBe(204);
    //     });    
    // }
}