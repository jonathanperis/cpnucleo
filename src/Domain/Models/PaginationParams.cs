namespace Domain.Models;

/// <summary>
/// Paging, search and sorting for list queries. Setters only store values: binders and
/// serializers (FastEndpoints query binding, MessagePack over gRPC) must be able to create the
/// object from any input. <see cref="Problems"/> and <see cref="Require"/> enforce the bounds, so
/// both transports reject the same invalid requests with the same field-level messages instead of
/// failing inside a deserializer.
/// </summary>
public class PaginationParams
{
    public const int MaximumPageSize = 100;
    public const int MaximumPageNumber = int.MaxValue / MaximumPageSize;
    public const int MaximumSearchLength = 128;
    private int? _pageNumber;
    private int? _pageSize;
    private string? _sortColumn;
    private string? _sortOrder;
    private string? _search;

    /// <summary>Comma-separated UUIDs (at most <see cref="MaximumPageSize"/>) restricting the result.</summary>
    public string? Ids { get; set; }

    public string? Search
    {
        get => _search;
        set => _search = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    public int? PageNumber
    {
        get => _pageNumber ?? 1;
        set => _pageNumber = value;
    }

    public int? PageSize
    {
        get => _pageSize ?? 10;
        set => _pageSize = value;
    }

    public string? SortColumn
    {
        get => _sortColumn ?? "Id";
        set => _sortColumn = value;
    }

    public string? SortOrder
    {
        get => (_sortOrder?.ToUpperInvariant() == "DESC") ? "DESC" : "ASC";
        set => _sortOrder = value;
    }

    public int Offset => (PageNumber.GetValueOrDefault(1) - 1) * PageSize.GetValueOrDefault(10);

    /// <summary>
    /// Every bound the values violate, keyed by the flat query field name (<c>pageNumber</c>,
    /// <c>pageSize</c>, <c>search</c>, <c>ids</c>). Empty when the request is valid.
    /// </summary>
    public IReadOnlyList<(string Field, string Message)> Problems()
    {
        var problems = new List<(string, string)>();
        if (PageNumber is < 1 or > MaximumPageNumber)
            problems.Add(("pageNumber", $"PageNumber must be between 1 and {MaximumPageNumber}."));
        if (PageSize is < 1 or > MaximumPageSize)
            problems.Add(("pageSize", $"PageSize must be between 1 and {MaximumPageSize}."));
        if (_search?.Length > MaximumSearchLength)
            problems.Add(("search", $"Search must be at most {MaximumSearchLength} characters."));
        var ids = SplitIds();
        if (ids.Length > MaximumPageSize || ids.Any(id => !Guid.TryParse(id, out _)))
            problems.Add(("ids", $"Ids must contain at most {MaximumPageSize} comma-separated UUIDs."));
        return problems;
    }

    /// <summary>Returns <paramref name="pagination"/> when present and valid, otherwise throws a <see cref="DomainException"/>.</summary>
    public static PaginationParams Require(PaginationParams? pagination)
    {
        if (pagination is null) throw new DomainException("Pagination is required.", "pagination");
        if (pagination.Problems() is [var first, ..]) throw new DomainException(first.Message, first.Field);
        return pagination;
    }

    /// <summary>The distinct UUIDs of <see cref="Ids"/>; call after <see cref="Require"/>.</summary>
    public Guid[] GetIds() => SplitIds()
        .Select(value => Guid.TryParse(value, out var id) ? id : Guid.Empty)
        .Where(id => id != Guid.Empty)
        .Distinct().ToArray();

    /// <summary>
    /// The search text as a case-insensitive "contains" LIKE pattern, with LIKE wildcards escaped
    /// so <c>%</c>, <c>_</c> and <c>\</c> match literally. Use with <c>ESCAPE '\'</c>.
    /// </summary>
    public string? GetSearchPattern() => _search is null
        ? null
        : "%" + _search.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_") + "%";

    private string[] SplitIds() =>
        Ids?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [];
}
