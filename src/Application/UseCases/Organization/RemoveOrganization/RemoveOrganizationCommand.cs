namespace Application.UseCases.Organization.RemoveOrganization;

public sealed record RemoveOrganizationCommand(Ulid Id) : BaseCommand, IRequest<OperationResult>;
