using WebApi.Endpoints.Organization.CreateOrganization;

namespace WebApi.Integration.Tests.Endpoints.Organization;

[Collection<OrganizationCollection>]
[Priority(1)]
public class CreateOrganizationTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _organizationId = state.OrganizationId;

    [Fact, Priority(1)]
    public async Task Organizations_ShouldCreateAnOrganization()
    {
        var (rsp, err) = await app.Client.POSTAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "New organization",
            Description = "New organization description",
            Id = _organizationId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
