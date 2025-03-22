namespace Application.UseCases.Organization.GetOrganizationById;

public sealed record GetOrganizationByIdQuery(Guid Id) : BaseQuery, IRequest<GetOrganizationByIdQueryViewModel>;
