namespace Application.UseCases.Organization.RemoveOrganization;

public sealed record RemoveOrganizationCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;
