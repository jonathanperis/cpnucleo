namespace Application.UseCases.System.RemoveSystem;

public sealed record RemoveSystemCommand(Ulid Id) : BaseCommand, IRequest<OperationResult>;
