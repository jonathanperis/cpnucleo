namespace Application.UseCases.Organization.ListOrganization;

public sealed record ListOrganizationQuery(PaginationParams Pagination) : BaseQuery, IRequest<ListOrganizationQueryViewModel>;
