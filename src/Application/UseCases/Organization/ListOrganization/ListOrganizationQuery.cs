namespace Application.UseCases.Organization.ListOrganization;

public sealed record ListOrganizationQuery() : BaseQuery, IRequest<ListOrganizationQueryViewModel>;
