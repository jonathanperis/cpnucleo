using WebApi.Endpoints.Organization.RemoveOrganization;

namespace WebApi.Integration.Tests.Endpoints.Organization;

[Collection<OrganizationCollection>]
[Priority(5)]
public class RemoveOrganizationTests(WebAppFixture app) : TestBase
{
    private readonly Guid _organizationId = app.OrganizationId;

    [Fact, Priority(1)]
    public async Task Organizations_ShouldDeleteAnOrganization()
    {
        var (rsp, err) = await app.Client.DELETEAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Ids = [_organizationId]
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
