using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Security;

/// <summary>
/// Applies <see cref="IAccessGuard"/> to every EF Core write, checking both the original row (what
/// the caller is allowed to touch) and the new values (where the caller is moving it to).
/// </summary>
public sealed class AccessGuardInterceptor(IAccessGuard accessGuard) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is { } context) await AuthorizeAsync(context, cancellationToken).ConfigureAwait(false);
        return result;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        // Synchronous saves are only used by tooling; authorize with the same rules.
        if (eventData.Context is { } context) AuthorizeAsync(context, CancellationToken.None).GetAwaiter().GetResult();
        return result;
    }

    private async Task AuthorizeAsync(DbContext context, CancellationToken cancellationToken)
    {
        var entries = context.ChangeTracker.Entries<BaseEntity>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToArray();

        foreach (var entry in entries)
        {
            var current = ResourceAccess.TargetOf(entry.Entity);
            if (entry.State == EntityState.Added)
            {
                await accessGuard.EnsureCanWriteAsync(current, AccessOperation.Create, cancellationToken).ConfigureAwait(false);
                continue;
            }

            var original = ResourceAccess.TargetOf((BaseEntity)entry.OriginalValues.ToObject());
            await accessGuard.EnsureCanWriteAsync(original, AccessOperation.Modify, cancellationToken).ConfigureAwait(false);
            if (current != original)
                await accessGuard.EnsureCanWriteAsync(current, AccessOperation.Modify, cancellationToken).ConfigureAwait(false);
        }
    }
}
