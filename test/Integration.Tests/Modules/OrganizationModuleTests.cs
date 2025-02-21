using Domain.Models;

namespace Integration.Tests.Modules;

// public class OrganizationTests(WebAppFixture fixture) : ScenarioContext(fixture)
public class OrganizationModuleTests(WebAppFixture fixture) : IClassFixture<WebAppFixture>
{
    private readonly IAlbaHost _host = fixture.AlbaHost;
    
    [Fact]
    public async Task Organizations_ShouldReturnAllOrganizations()
    {
        // Act/Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC"}).ToUrl("/api/organizations");
            s.StatusCodeShouldBeOk();
        });
    }
    
    [Fact]
    public async Task Organizations_ShouldGetAnOrganization()
    {
        // Arrange
        var response1 = await _host.Scenario(s =>
        {
            s.Get.Json(new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC"}).ToUrl("/api/organizations");
            s.StatusCodeShouldBeOk();
        });
        
        var organizations = await response1.ReadAsJsonAsync<ListOrganizationQueryViewModel>();
        var organization = organizations?.Result?.Data?.First();
        
        // Act
        var response2 = await _host.Scenario(s =>
        {
            s.Get.Json(new GetOrganizationByIdQuery(organization!.Id)).ToUrl("/api/organizations");
            s.StatusCodeShouldBeOk();
        });        
     
        var result = await response2.ReadAsJsonAsync<GetOrganizationByIdQueryViewModel>();
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.Equal(result?.Organization?.Id, organization?.Id);        
    }
}