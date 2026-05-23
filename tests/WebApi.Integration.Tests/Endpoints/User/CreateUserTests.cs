using WebApi.Endpoints.User.CreateUser;

namespace WebApi.Integration.Tests.Endpoints.User;

[Collection<UserCollection>]
[Priority(1)]
public class CreateUserTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _userId = state.UserId;

    [Fact, Priority(1)]
    public async Task Users_ShouldCreateAnUser()
    {
        var (rsp, err) = await app.Client.POSTAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "New user",
            Login = "newuser123",
            Password = "Password123!",
            Id = _userId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
