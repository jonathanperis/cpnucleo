namespace Domain.Models;

public record PaginatedResult<T>
{
    public IEnumerable<T>? Data { get; init; }
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}