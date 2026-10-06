using Microsoft.Extensions.Caching.Memory;

namespace IdentityApi.Security;

/// <summary>
/// Per-login brute-force protection that complements the per-IP rate limiter: after
/// <see cref="MaximumFailures"/> failed attempts within <see cref="Window"/>, the login is locked for
/// <see cref="Lockout"/> regardless of which addresses the attempts come from. State is per instance.
/// </summary>
public sealed class LoginThrottle(TimeProvider timeProvider) : IDisposable
{
    public const int MaximumFailures = 5;
    public static readonly TimeSpan Window = TimeSpan.FromMinutes(15);
    public static readonly TimeSpan Lockout = TimeSpan.FromMinutes(15);

    // Bounded so login spraying can't grow memory without limit.
    private readonly MemoryCache attempts = new(new MemoryCacheOptions { SizeLimit = 100_000 });
    private readonly object gate = new();

    /// <summary>How long the caller must wait before trying this login again, if it's locked.</summary>
    public TimeSpan? RetryAfter(string normalizedLogin)
    {
        lock (gate)
        {
            if (!attempts.TryGetValue(normalizedLogin, out Attempts? state) || state!.LockedUntil is not { } until) return null;
            var remaining = until - timeProvider.GetUtcNow();
            return remaining > TimeSpan.Zero ? remaining : null;
        }
    }

    public void RecordFailure(string normalizedLogin)
    {
        lock (gate)
        {
            var now = timeProvider.GetUtcNow();
            if (!attempts.TryGetValue(normalizedLogin, out Attempts? state) || now - state!.WindowStartedAt > Window || state.LockedUntil < now)
                state = new Attempts(now, 0, null);

            state = state with { Failures = state.Failures + 1 };
            if (state.Failures >= MaximumFailures) state = state with { LockedUntil = now + Lockout };
            attempts.Set(normalizedLogin, state, new MemoryCacheEntryOptions { Size = 1, AbsoluteExpirationRelativeToNow = Window + Lockout });
        }
    }

    public void RecordSuccess(string normalizedLogin)
    {
        lock (gate)
        {
            attempts.Remove(normalizedLogin);
        }
    }

    public void Dispose() => attempts.Dispose();

    private sealed record Attempts(DateTimeOffset WindowStartedAt, int Failures, DateTimeOffset? LockedUntil);
}
