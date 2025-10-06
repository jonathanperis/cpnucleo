using WebApi.Endpoints.Organization.GetOrganizationById;

namespace WebApi.Integration.Tests.Endpoints.Organization;

[Collection<OrganizationCollection>]
[Priority(3)]
public class GetOrganizationByIdTests(WebAppFixture app) : TestBase
{
    private readonly Guid _organizationId = app.OrganizationId;

    [Fact, Priority(1)]
    public async Task Organizations_ShouldGetAnOrganization()
    {
        var (rsp, err) = await app.Client.GETAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Id = _organizationId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
