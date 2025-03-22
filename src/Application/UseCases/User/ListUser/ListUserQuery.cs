namespace Application.UseCases.User.ListUser;

public sealed record ListUserQuery(PaginationParams Pagination) : BaseQuery, IRequest<ListUserQueryViewModel>;
