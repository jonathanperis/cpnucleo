using WebApi.Endpoints.Impediment.CreateImpediment;

namespace WebApi.Integration.Tests.Endpoints.Impediment;

[Collection<ImpedimentCollection>]
[Priority(1)]
public class CreateImpedimentTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _impedimentId = state.ImpedimentId;

    [Fact, Priority(1)]
    public async Task Impediments_ShouldCreateAnImpediment()
    {
        var (rsp, err) = await app.Client.POSTAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "New impediment",
            Id = _impedimentId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
