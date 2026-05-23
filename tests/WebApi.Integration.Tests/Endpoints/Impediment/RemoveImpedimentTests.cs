using WebApi.Endpoints.Impediment.RemoveImpediment;

namespace WebApi.Integration.Tests.Endpoints.Impediment;

[Collection<ImpedimentCollection>]
[Priority(5)]
public class RemoveImpedimentTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _impedimentId = state.ImpedimentId;

    [Fact, Priority(1)]
    public async Task Impediments_ShouldDeleteAnImpediment()
    {
        var (rsp, err) = await app.Client.DELETEAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Ids = [_impedimentId]
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
