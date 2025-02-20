namespace Integration.Tests.Modules;

// public class OrganizationTests(WebAppFixture fixture) : ScenarioContext(fixture)
public class OrganizationTests(WebAppFixture fixture) : IClassFixture<WebAppFixture>
{
    private readonly IAlbaHost _host = fixture.AlbaHost;
    
    [Fact]
    public async Task Organizations_ShouldReturnAllOrganizations()
    {
        // Arrange
        await _host.Scenario(s =>
        {
            s.Get.Json(new ListOrganizationQuery()).ToUrl("/api/organizations");
            s.StatusCodeShouldBeOk();
        });
    }
}