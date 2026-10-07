using Domain.Common;

namespace Domain.Models;

/// <summary>
/// Normalizes the ids of a batch removal or restore: distinct, non-empty, at most <see cref="MaximumCount"/>.
/// </summary>
public static class BatchIds
{
    public const int MaximumCount = 100;

    /// <param name="ids">The requested ids.</param>
    /// <param name="action">How the batch is described in the limit message ("removed", "restored").</param>
    public static Guid[] Normalize(IEnumerable<Guid>? ids, string action = "removed")
    {
        var distinct = (ids ?? []).Distinct().ToArray();
        if (distinct.Length == 0) throw new DomainException("At least one id is required.", "Ids");
        if (distinct.Length > MaximumCount) throw new DomainException($"At most {MaximumCount} ids can be {action} at once.", "Ids");
        if (distinct.Contains(Guid.Empty)) throw new DomainException("Ids must not contain empty values.", "Ids");
        return distinct;
    }
}
