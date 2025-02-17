namespace Application.UseCases.Organization.CreateOrganization;

public sealed record CreateOrganizationCommand(string Name, string Description, Guid Id = default) : BaseCommand, IRequest<OperationResult>;