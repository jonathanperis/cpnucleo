namespace Domain.Models;

public class PaginationParams
{
    private int? _pageNumber;
    private int? _pageSize;
    private string? _sortColumn;
    private string? _sortOrder;

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
        get => (_sortOrder?.ToUpper() == "DESC") ? "DESC" : "ASC";
        set => _sortOrder = value;
    }

    public int Offset => (PageNumber.GetValueOrDefault(1) - 1) * PageSize.GetValueOrDefault(10);
}