namespace Domain.Models;

public class PaginationParams
{
    private int? _pageNumber;
    private int? _pageSize;
    private string? _sortColumn;
    private string? _sortOrder;

    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be at least 1")]
    public int? PageNumber
    {
        get => _pageNumber ?? 1;
        set => _pageNumber = value;
    }

    [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
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
        get => (_sortOrder?.ToUpper() == "DESC") ? "DESC" : "ASC";
        set => _sortOrder = value;
    }

    public int Offset => (PageNumber.GetValueOrDefault(1) - 1) * PageSize.GetValueOrDefault(10);
}
