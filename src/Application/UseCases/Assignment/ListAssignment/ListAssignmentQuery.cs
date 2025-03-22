namespace Application.UseCases.Assignment.ListAssignment;

public sealed record ListAssignmentQuery(PaginationParams Pagination) : BaseQuery, IRequest<ListAssignmentQueryViewModel>;
