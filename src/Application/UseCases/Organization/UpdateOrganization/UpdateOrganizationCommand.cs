namespace Application.UseCases.Organization.UpdateOrganization;

public sealed record UpdateOrganizationCommand(Ulid Id, string Name, string Description) : BaseCommand, IRequest<OperationResult>;
