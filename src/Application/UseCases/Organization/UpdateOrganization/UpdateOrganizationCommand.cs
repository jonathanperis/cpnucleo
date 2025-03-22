namespace Application.UseCases.Organization.UpdateOrganization;

public sealed record UpdateOrganizationCommand(Guid Id, string Name, string Description) : BaseCommand, IRequest<OperationResult>;
