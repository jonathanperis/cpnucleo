using WebApi.Endpoints.Organization.UpdateOrganization;

namespace WebApi.Integration.Tests.Endpoints.Organization;

[Collection<OrganizationCollection>]
[Priority(4)]
public class UpdateOrganizationTests(WebAppFixture app) : TestBase
{
    private readonly Guid _organizationId = app.OrganizationId;

    [Fact, Priority(1)]
    public async Task Organizations_ShouldUpdateAnOrganization()
    {
        var (rsp, err) = await app.Client.PATCHAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "New organization",
            Description = "New organization description",
            Id = _organizationId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
