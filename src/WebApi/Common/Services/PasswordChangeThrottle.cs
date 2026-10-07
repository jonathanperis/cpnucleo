using Microsoft.Extensions.Caching.Memory;

namespace WebApi.Common.Services;

/// <summary>
/// Per-account protection for changing one's own password (<c>POST /me/password</c>), complementing
/// the per-IP rate limiter: after <see cref="MaximumFailures"/> wrong current passwords within
/// <see cref="Window"/>, the account's password changes are locked for <see cref="Lockout"/>, so a
/// stolen access token can't be used to guess the password. State is per instance.
/// </summary>
public sealed class PasswordChangeThrottle(TimeProvider timeProvider) : IDisposable
{
    public const int MaximumFailures = 5;
    public static readonly TimeSpan Window = TimeSpan.FromMinutes(15);
    public static readonly TimeSpan Lockout = TimeSpan.FromMinutes(15);

    // Bounded so the cache can't grow without limit.
    private readonly MemoryCache attempts = new(new MemoryCacheOptions { SizeLimit = 100_000 });
    private readonly object gate = new();

    /// <summary>How long the account must wait before trying again, if it's locked.</summary>
    public TimeSpan? RetryAfter(Guid userId)
    {
        lock (gate)
        {
            if (!attempts.TryGetValue(userId, out Attempts? state) || state!.LockedUntil is not { } until) return null;
            var remaining = until - timeProvider.GetUtcNow();
            return remaining > TimeSpan.Zero ? remaining : null;
        }
    }

    public void RecordFailure(Guid userId)
    {
        lock (gate)
        {
            var now = timeProvider.GetUtcNow();
            if (!attempts.TryGetValue(userId, out Attempts? state) || now - state!.WindowStartedAt > Window || state.LockedUntil < now)
                state = new Attempts(now, 0, null);

            state = state with { Failures = state.Failures + 1 };
            if (state.Failures >= MaximumFailures) state = state with { LockedUntil = now + Lockout };
            attempts.Set(userId, state, new MemoryCacheEntryOptions { Size = 1, AbsoluteExpirationRelativeToNow = Window + Lockout });
        }
    }

    public void RecordSuccess(Guid userId)
    {
        lock (gate)
        {
            attempts.Remove(userId);
        }
    }

    public void Dispose() => attempts.Dispose();

    private sealed record Attempts(DateTimeOffset WindowStartedAt, int Failures, DateTimeOffset? LockedUntil);
}
