namespace Application.UseCases.UserAssignment.ListUserAssignment;

public sealed record ListUserAssignmentQuery(PaginationParams Pagination) : BaseQuery, IRequest<ListUserAssignmentQueryViewModel>;
