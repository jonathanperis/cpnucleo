using WebApi.Endpoints.Impediment.GetImpedimentById;

namespace WebApi.Integration.Tests.Endpoints.Impediment;

[Collection<ImpedimentCollection>]
[Priority(3)]
public class GetImpedimentByIdTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _impedimentId = state.ImpedimentId;

    [Fact, Priority(1)]
    public async Task Impediments_ShouldGetAnImpediment()
    {
        var (rsp, err) = await app.Client.GETAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Id = _impedimentId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
