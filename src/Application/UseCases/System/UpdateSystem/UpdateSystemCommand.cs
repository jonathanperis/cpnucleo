namespace Application.UseCases.System.UpdateSystem;

public sealed record UpdateSystemCommand(Ulid Id, string Name, string Description) : BaseCommand, IRequest<OperationResult>;
