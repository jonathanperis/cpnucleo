using WebApi.Endpoints.User.RemoveUser;

namespace WebApi.Integration.Tests.Endpoints.User;

[Collection<UserCollection>]
[Priority(5)]
public class RemoveUserTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _userId = state.UserId;

    [Fact, Priority(1)]
    public async Task Users_ShouldDeleteAnUser()
    {
        var (rsp, err) = await app.Client.DELETEAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Ids = [_userId]
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
