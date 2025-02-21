namespace Application.UseCases.AssignmentType.ListAssignmentType;

public sealed record ListAssignmentTypeQuery(PaginationParams Pagination) : BaseQuery, IRequest<ListAssignmentTypeQueryViewModel>;
