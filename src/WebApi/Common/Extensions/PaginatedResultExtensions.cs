namespace WebApi.Common.Extensions;

/// <summary>
/// Extension methods for PaginatedResult mapping.
/// </summary>
public static class PaginatedResultExtensions
{
    /// <summary>
    /// Maps a paginated result from entity type to DTO type using the provided mapping function.
    /// </summary>
    /// <typeparam name="TEntity">The source entity type.</typeparam>
    /// <typeparam name="TDto">The target DTO type.</typeparam>
    /// <param name="result">The paginated result containing entities.</param>
    /// <param name="mapper">Function to map from entity to DTO.</param>
    /// <returns>A new paginated result with mapped DTOs.</returns>
    public static PaginatedResult<TDto?> MapToDto<TEntity, TDto>(
        this PaginatedResult<TEntity?> result,
        Func<TEntity?, TDto?> mapper)
    {
        return new PaginatedResult<TDto?>
        {
            Data = result.Data?.Select(mapper).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
