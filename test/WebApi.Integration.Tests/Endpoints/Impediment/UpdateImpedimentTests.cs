using WebApi.Endpoints.Impediment.UpdateImpediment;

namespace WebApi.Integration.Tests.Endpoints.Impediment;

[Collection<ImpedimentCollection>]
[Priority(4)]
public class UpdateImpedimentTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _impedimentId = state.ImpedimentId;

    [Fact, Priority(1)]
    public async Task Impediments_ShouldUpdateAnImpediment()
    {
        var (rsp, err) = await app.Client.PATCHAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "Updated impediment",
            Id = _impedimentId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
