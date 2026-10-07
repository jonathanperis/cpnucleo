namespace WebApi.Unit.Tests.Common.Services;

public class PasswordChangeThrottleTests
{
    private sealed class MutableTimeProvider(DateTimeOffset now) : TimeProvider
    {
        public DateTimeOffset Now { get; set; } = now;

        public override DateTimeOffset GetUtcNow() => Now;
    }

    [Test]
    public void AnAccount_IsLockedAfterRepeatedWrongPasswords_AndUnlocksAfterTheLockout()
    {
        var time = new MutableTimeProvider(DateTimeOffset.UtcNow);
        using var throttle = new PasswordChangeThrottle(time);
        var jane = Guid.NewGuid();

        for (var attempt = 0; attempt < PasswordChangeThrottle.MaximumFailures; attempt++)
        {
            throttle.RetryAfter(jane).ShouldBeNull();
            throttle.RecordFailure(jane);
        }

        throttle.RetryAfter(jane).ShouldNotBeNull().ShouldBeGreaterThan(TimeSpan.Zero);
        throttle.RetryAfter(Guid.NewGuid()).ShouldBeNull("lockouts are per account");

        time.Now += PasswordChangeThrottle.Lockout + TimeSpan.FromSeconds(1);
        throttle.RetryAfter(jane).ShouldBeNull();
    }

    [Test]
    public void ACorrectPassword_ClearsEarlierFailures()
    {
        using var throttle = new PasswordChangeThrottle(TimeProvider.System);
        var jane = Guid.NewGuid();
        for (var attempt = 0; attempt < PasswordChangeThrottle.MaximumFailures - 1; attempt++) throttle.RecordFailure(jane);

        throttle.RecordSuccess(jane);
        throttle.RecordFailure(jane);

        throttle.RetryAfter(jane).ShouldBeNull();
    }
}
