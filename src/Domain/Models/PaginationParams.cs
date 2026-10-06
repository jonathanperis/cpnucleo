namespace Domain.Models;

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
    private string? _ids;

    public string? Ids
    {
        get => _ids;
        set
        {
            var values = value?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [];
            if (values.Length > MaximumPageSize || values.Any(id => !Guid.TryParse(id, out _)))
                throw new DomainException($"Ids must contain at most {MaximumPageSize} comma-separated UUIDs.", nameof(Ids));
            _ids = value;
        }
    }

    // Values reaching this point were validated by the setter; TryParse also keeps a payload that
    // bypassed the setter (e.g. a hand-crafted gRPC message) from failing as an unexpected error.
    public Guid[] GetIds() => _ids?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(value => Guid.TryParse(value, out var id) ? id : Guid.Empty)
        .Where(id => id != Guid.Empty)
        .Distinct().ToArray() ?? [];

    public string? Search
    {
        get => _search;
        set
        {
            if (value?.Length > MaximumSearchLength)
                throw new DomainException($"Search must be at most {MaximumSearchLength} characters.", nameof(Search));
            _search = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }

    /// <summary>
    /// The search text as a case-insensitive "contains" LIKE pattern, with LIKE wildcards escaped
    /// so <c>%</c>, <c>_</c> and <c>\</c> match literally. Use with <c>ESCAPE '\'</c>.
    /// </summary>
    public string? GetSearchPattern() => _search is null
        ? null
        : "%" + _search.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_") + "%";

    public int? PageNumber
    {
        get => _pageNumber ?? 1;
        set
        {
            if (value is < 1 or > MaximumPageNumber)
                throw new DomainException($"PageNumber must be between 1 and {MaximumPageNumber}.", nameof(PageNumber));
            _pageNumber = value;
        }
    }

    public int? PageSize
    {
        get => _pageSize ?? 10;
        set
        {
            if (value is < 1 or > MaximumPageSize)
                throw new DomainException($"PageSize must be between 1 and {MaximumPageSize}.", nameof(PageSize));
            _pageSize = value;
        }
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
}
