namespace Integration.Tests.Modules;

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
        
        var result1 = await response1.ReadAsJsonAsync<PaginatedResult<OrganizationDto>>();
        var organization = result1?.Data?.First();
        
        // Act
        var response2 = await _host.GetAsJson<OrganizationDto>("/api/organizations/" + organization!.Id);
        
        // Assert
        Assert.NotNull(response2);
        Assert.Equal(response2?.Id, organization?.Id);        
    }
}