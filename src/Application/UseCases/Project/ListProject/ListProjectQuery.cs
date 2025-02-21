namespace Application.UseCases.Project.ListProject;

public sealed record ListProjectQuery(PaginationParams Pagination) : IRequest<ListProjectQueryViewModel>;
