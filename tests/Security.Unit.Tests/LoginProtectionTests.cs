using IdentityApi.Security;

namespace Security.Unit.Tests;

/// <summary>The sign-in protections the identity server keeps from the previous login endpoint.</summary>
public class LoginProtectionTests
{
    [Test]
    public void PasswordChecks_SpendTheSameHashingWorkForUnknownLogins()
    {
        var hasher = A.Fake<IPasswordHasher>();
        A.CallTo(() => hasher.Hash(A<string>._)).Returns(new PasswordHash("$argon2id$unmatchable", string.Empty));
        A.CallTo(() => hasher.Verify("Password@123", "$argon2id$unmatchable")).Returns(true);
        var check = new TimingSafePasswordCheck(hasher);

        check.Verify("Password@123", storedHash: null).ShouldBeFalse("a matching dummy hash must still fail");
        A.CallTo(() => hasher.Verify("Password@123", "$argon2id$unmatchable")).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void ALogin_IsLockedAfterRepeatedFailures_AndUnlocksAfterTheLockout()
    {
        var time = new MutableTimeProvider(DateTimeOffset.UtcNow);
        using var throttle = new LoginThrottle(time);

        for (var attempt = 0; attempt < LoginThrottle.MaximumFailures; attempt++)
        {
            throttle.RetryAfter("jane").ShouldBeNull();
            throttle.RecordFailure("jane");
        }

        throttle.RetryAfter("jane").ShouldNotBeNull().ShouldBeGreaterThan(TimeSpan.Zero);
        throttle.RetryAfter("john").ShouldBeNull("lockouts are per login");

        time.Advance(LoginThrottle.Lockout + TimeSpan.FromSeconds(1));
        throttle.RetryAfter("jane").ShouldBeNull();
    }

    [Test]
    public void ASuccessfulSignIn_ClearsEarlierFailures()
    {
        using var throttle = new LoginThrottle(TimeProvider.System);
        for (var attempt = 0; attempt < LoginThrottle.MaximumFailures - 1; attempt++) throttle.RecordFailure("jane");

        throttle.RecordSuccess("jane");
        throttle.RecordFailure("jane");

        throttle.RetryAfter("jane").ShouldBeNull();
    }
}
