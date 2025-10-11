using WebApi.Endpoints.User.GetUserById;

namespace WebApi.Integration.Tests.Endpoints.User;

[Collection<UserCollection>]
[Priority(3)]
public class GetUserByIdTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _userId = state.UserId;

    [Fact, Priority(1)]
    public async Task Users_ShouldGetAnUser()
    {
        var (rsp, err) = await app.Client.GETAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Id = _userId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
