namespace Application.UseCases.Organization.CreateOrganization;

public sealed record CreateOrganizationCommand(string Name, string Description, Ulid Id = default) : BaseCommand, IRequest<OperationResult>;