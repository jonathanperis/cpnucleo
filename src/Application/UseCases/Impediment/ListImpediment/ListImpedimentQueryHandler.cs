namespace Application.UseCases.Impediment.ListImpediment;
 
// EF Core
public sealed class ListImpedimentQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<ListImpedimentQuery, ListImpedimentQueryViewModel>
{
    public async ValueTask<ListImpedimentQueryViewModel> Handle(ListImpedimentQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Impediments!.AsQueryable();
        
        var validSortColumn = ValidateSortColumn(request.Pagination.SortColumn);
        var validSortOrder = request.Pagination.SortOrder?.ToUpper() == "DESC" ? "DESC" : "ASC";
        
        query = ApplySorting(query, validSortColumn, validSortOrder);

        var totalCount = await query!.CountAsync(cancellationToken);

        var impediments = await query!
            .Skip(request.Pagination.Offset)
            .Take(request.Pagination.PageSize.GetValueOrDefault())
            .ToListAsync(cancellationToken);

        var operationResult = impediments.Count > 0 ? OperationResult.Success : OperationResult.NotFound;

        var result =  new PaginatedResult<ImpedimentDto?>
        {
            Data = impediments.Select(x => x?.MapToDto()).ToList(),
            TotalCount = totalCount,
            PageNumber = request.Pagination.PageNumber.GetValueOrDefault(),
            PageSize = request.Pagination.PageSize.GetValueOrDefault()
        };
        
        return new ListImpedimentQueryViewModel(operationResult, result);
    }
    
    private static string ValidateSortColumn(string? column)
    {
        if (string.IsNullOrWhiteSpace(column))
            return "Id";

        var properties = typeof(Domain.Entities.Impediment).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        return properties.Any(p => p.Name.Equals(column, StringComparison.OrdinalIgnoreCase)) 
            ? column 
            : "Id";
    }
    
    private static IQueryable<Domain.Entities.Impediment>? ApplySorting(IQueryable<Domain.Entities.Impediment>? query, string sortColumn, string sortOrder)
    {
        return query?.OrderBy($"{sortColumn} {sortOrder}");
    }    
}
