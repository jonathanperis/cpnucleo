namespace Infrastructure.Security;

/// <summary>
/// Allows every write. Only for trusted in-process tools and tests that use repositories directly
/// with <see cref="StaticCurrentUser.System"/>; request handling always uses <see cref="AccessGuard"/>.
/// </summary>
public sealed class TrustedAccessGuard : IAccessGuard
{
    public static TrustedAccessGuard Instance { get; } = new();

    private TrustedAccessGuard()
    {
    }

    public Task EnsureCanWriteAsync(AccessTarget target, AccessOperation operation, DatabaseSession? session = null, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
