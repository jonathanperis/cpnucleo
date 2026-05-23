using WebApi.Endpoints.User.UpdateUser;

namespace WebApi.Integration.Tests.Endpoints.User;

[Collection<UserCollection>]
[Priority(4)]
public class UpdateUserTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _userId = state.UserId;

    [Fact, Priority(1)]
    public async Task Users_ShouldUpdateAnUser()
    {
        var (rsp, err) = await app.Client.PATCHAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "Updated user",
            Password = "Password123!",
            Id = _userId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
