namespace Application.UseCases.UserProject.ListUserProject;

public sealed record ListUserProjectQuery(PaginationParams Pagination) : BaseQuery, IRequest<ListUserProjectQueryViewModel>;
