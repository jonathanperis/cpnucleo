namespace Domain.Models;

public class PaginationParams
{
    public const int MaximumPageSize = 100;
    public const int MaximumPageNumber = int.MaxValue / MaximumPageSize;
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
                throw new ArgumentException("Ids must contain at most 100 comma-separated UUIDs.", nameof(Ids));
            _ids = value;
        }
    }

    public Guid[] GetIds() => _ids?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(Guid.Parse).ToArray() ?? [];

    public string? Search
    {
        get => _search;
        set
        {
            if (value?.Length > 128) throw new ArgumentOutOfRangeException(nameof(Search));
            _search = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }

    public int? PageNumber
    {
        get => _pageNumber ?? 1;
        set
        {
            if (value is < 1 or > MaximumPageNumber) throw new ArgumentOutOfRangeException(nameof(PageNumber));
            _pageNumber = value;
        }
    }

    public int? PageSize
    {
        get => _pageSize ?? 10;
        set
        {
            if (value is < 1 or > MaximumPageSize) throw new ArgumentOutOfRangeException(nameof(PageSize));
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
        get => (_sortOrder?.ToUpper() == "DESC") ? "DESC" : "ASC";
        set => _sortOrder = value;
    }

    public int Offset => (PageNumber.GetValueOrDefault(1) - 1) * PageSize.GetValueOrDefault(10);
}
