using System.IdentityModel.Tokens.Jwt;
using IdentityApi.Security;
using LoginEndpoint = IdentityApi.Endpoints.Login.Endpoint;
using LoginRequest = IdentityApi.Endpoints.Login.Request;

namespace Security.Unit.Tests;

[TestFixture]
public class LoginEndpointTests
{
    private const string StoredHash = "$argon2id$stored-hash";

    [Test]
    public async Task Login_IssuesATokenBoundToTheAccountCredentials()
    {
        await using var db = TestSupport.CreateDbContext();
        var user = await AddUserAsync(db, "jane");
        var hasher = A.Fake<IPasswordHasher>();
        A.CallTo(() => hasher.Verify("Password@123", StoredHash)).Returns(true);
        var endpoint = CreateEndpoint(db, hasher, TestSupport.Configuration(adminLogins: "JANE"));

        await endpoint.HandleAsync(new LoginRequest { Login = " Jane ", Password = "Password@123" }, default);

        endpoint.HttpContext.Response.StatusCode.ShouldBe(200);
        var token = new JwtSecurityTokenHandler().ReadJwtToken(endpoint.Response.Token);
        token.Subject.ShouldBe(user.Id.ToString());
        token.Claims.ShouldContain(c => c.Type == CpnucleoClaimTypes.SecurityStamp && c.Value == SecurityStamp.Compute(user));
        token.Claims.ShouldContain(c => c.Type == CpnucleoClaimTypes.Admin && c.Value == "true");
        token.ValidTo.ShouldBe(DateTime.UtcNow.Add(TokenIssuer.AccessTokenLifetime), TimeSpan.FromSeconds(5));
        A.CallTo(() => hasher.Verify("Password@123", StoredHash)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task Login_RejectsAWrongPassword()
    {
        await using var db = TestSupport.CreateDbContext();
        await AddUserAsync(db, "jane");
        var hasher = A.Fake<IPasswordHasher>();
        var endpoint = CreateEndpoint(db, hasher, TestSupport.Configuration());

        await endpoint.HandleAsync(new LoginRequest { Login = "jane", Password = "WrongPassword@123" }, default);

        endpoint.HttpContext.Response.StatusCode.ShouldBe(404);
    }

    [Test]
    public async Task Login_SpendsTheSameHashingWorkForUnknownLogins()
    {
        await using var db = TestSupport.CreateDbContext();
        var hasher = A.Fake<IPasswordHasher>();
        A.CallTo(() => hasher.Hash(A<string>._)).Returns(new PasswordHash("$argon2id$unmatchable", string.Empty));
        var endpoint = CreateEndpoint(db, hasher, TestSupport.Configuration());

        await endpoint.HandleAsync(new LoginRequest { Login = "nobody", Password = "Password@123" }, default);

        endpoint.HttpContext.Response.StatusCode.ShouldBe(404);
        A.CallTo(() => hasher.Verify("Password@123", "$argon2id$unmatchable")).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task Login_NeverPicksAnAccountForAnAmbiguousLogin()
    {
        await using var db = TestSupport.CreateDbContext();
        await AddUserAsync(db, "jane");
        await AddUserAsync(db, " JANE ");
        var hasher = A.Fake<IPasswordHasher>();
        A.CallTo(() => hasher.Verify(A<string>._, StoredHash)).Returns(true);
        var endpoint = CreateEndpoint(db, hasher, TestSupport.Configuration());

        await endpoint.HandleAsync(new LoginRequest { Login = "jane", Password = "Password@123" }, default);

        endpoint.HttpContext.Response.StatusCode.ShouldBe(404);
        A.CallTo(() => hasher.Verify(A<string>._, StoredHash)).MustNotHaveHappened();
    }

    [Test]
    public async Task Login_LocksALoginAfterRepeatedFailuresFromAnyAddress()
    {
        await using var db = TestSupport.CreateDbContext();
        await AddUserAsync(db, "jane");
        var hasher = A.Fake<IPasswordHasher>();
        A.CallTo(() => hasher.Verify("Password@123", StoredHash)).Returns(true);
        var time = new MutableTimeProvider(DateTimeOffset.UtcNow);
        using var throttle = new LoginThrottle(time);

        for (var attempt = 0; attempt < LoginThrottle.MaximumFailures; attempt++)
        {
            var failing = CreateEndpoint(db, hasher, TestSupport.Configuration(), throttle, time);
            await failing.HandleAsync(new LoginRequest { Login = "jane", Password = "Wrong@1234" }, default);
            failing.HttpContext.Response.StatusCode.ShouldBe(404);
        }

        var locked = CreateEndpoint(db, hasher, TestSupport.Configuration(), throttle, time);
        await locked.HandleAsync(new LoginRequest { Login = "JANE", Password = "Password@123" }, default);
        locked.HttpContext.Response.StatusCode.ShouldBe(429);
        int.Parse(locked.HttpContext.Response.Headers.RetryAfter.ToString()).ShouldBeGreaterThan(0);

        time.Advance(LoginThrottle.Lockout + TimeSpan.FromSeconds(1));
        var afterLockout = CreateEndpoint(db, hasher, TestSupport.Configuration(), throttle, time);
        await afterLockout.HandleAsync(new LoginRequest { Login = "jane", Password = "Password@123" }, default);
        afterLockout.HttpContext.Response.StatusCode.ShouldBe(200);
    }

    [TestCase(257, 8, false)]
    [TestCase(10, 129, false)]
    [TestCase(256, 128, true)]
    public void Validator_BoundsInputsBeforeHashing(int loginLength, int passwordLength, bool valid)
    {
        var result = new LoginRequest.Validator().Validate(new LoginRequest
        {
            Login = new string('l', loginLength),
            Password = new string('p', passwordLength)
        });

        result.IsValid.ShouldBe(valid);
    }

    private static async Task<User> AddUserAsync(ApplicationDbContext db, string login)
    {
        var user = User.Create("Jane", login, new PasswordHash(StoredHash, string.Empty));
        db.Users!.Add(user);
        await db.SaveChangesAsync(default);
        return user;
    }

    private static LoginEndpoint CreateEndpoint(ApplicationDbContext db, IPasswordHasher hasher,
        Microsoft.Extensions.Configuration.IConfiguration configuration, LoginThrottle? throttle = null, TimeProvider? time = null) =>
        Factory.Create<LoginEndpoint>(db, new TimingSafePasswordCheck(hasher),
            throttle ?? new LoginThrottle(TimeProvider.System), TestSupport.Issuer(configuration, time));
}
