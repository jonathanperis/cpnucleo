namespace Application.UseCases.Impediment.ListImpediment;

public sealed record ListImpedimentQuery(PaginationParams Pagination) : BaseQuery, IRequest<ListImpedimentQueryViewModel>;
