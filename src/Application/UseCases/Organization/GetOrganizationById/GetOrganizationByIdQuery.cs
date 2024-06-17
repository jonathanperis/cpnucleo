namespace Application.UseCases.Organization.GetOrganizationById;

public sealed record GetOrganizationByIdQuery(Ulid Id) : BaseQuery, IRequest<GetOrganizationByIdQueryViewModel>;
