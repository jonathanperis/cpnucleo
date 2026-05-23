using LoginEndpoint = IdentityApi.Endpoints.Login.Endpoint;
using LoginRequest = IdentityApi.Endpoints.Login.Request;

namespace Security.Unit.Tests;

[TestFixture]
public class LoginEndpointPasswordVerificationTests
{
    [Test]
    public async Task Login_ShouldUsePasswordHasherForStoredArgon2HashAsync()
    {
        await using var dbContext = CreateDbContext();
        var user = User.Create("Jane", "jane", new PasswordHash("$argon2id$stored-hash", string.Empty));
        await dbContext.Users!.AddAsync(user);
        await dbContext.SaveChangesAsync(default);

        var passwordHasher = A.Fake<IPasswordHasher>();
        A.CallTo(() => passwordHasher.Verify("Password@123", "$argon2id$stored-hash")).Returns(true);

        var endpoint = Factory.Create<LoginEndpoint>(dbContext, passwordHasher);

        var exception = await Should.ThrowAsync<InvalidOperationException>(() =>
            endpoint.HandleAsync(new LoginRequest { Login = "jane", Password = "Password@123" }, default));

        exception.Message.ShouldContain("SigningKey");
        A.CallTo(() => passwordHasher.Verify("Password@123", "$argon2id$stored-hash")).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task Login_ShouldRejectWrongPasswordWhenHasherRejectsAsync()
    {
        await using var dbContext = CreateDbContext();
        var user = User.Create("Jane", "jane", new PasswordHash("$argon2id$stored-hash", string.Empty));
        await dbContext.Users!.AddAsync(user);
        await dbContext.SaveChangesAsync(default);

        var passwordHasher = A.Fake<IPasswordHasher>();
        A.CallTo(() => passwordHasher.Verify("WrongPassword@123", "$argon2id$stored-hash")).Returns(false);

        var endpoint = Factory.Create<LoginEndpoint>(dbContext, passwordHasher);

        await endpoint.HandleAsync(new LoginRequest { Login = "jane", Password = "WrongPassword@123" }, default);

        endpoint.HttpContext.Response.StatusCode.ShouldBe(404);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
}
