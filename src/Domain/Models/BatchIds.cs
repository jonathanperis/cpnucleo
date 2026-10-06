using Domain.Common;

namespace Domain.Models;

/// <summary>
/// Normalizes the ids of a batch removal: distinct, non-empty, at most <see cref="MaximumCount"/>.
/// </summary>
public static class BatchIds
{
    public const int MaximumCount = 100;

    public static Guid[] Normalize(IEnumerable<Guid>? ids)
    {
        var distinct = (ids ?? []).Distinct().ToArray();
        if (distinct.Length == 0) throw new DomainException("At least one id is required.", "Ids");
        if (distinct.Length > MaximumCount) throw new DomainException($"At most {MaximumCount} ids can be removed at once.", "Ids");
        if (distinct.Contains(Guid.Empty)) throw new DomainException("Ids must not contain empty values.", "Ids");
        return distinct;
    }
}
