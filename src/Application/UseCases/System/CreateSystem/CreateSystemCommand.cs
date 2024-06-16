namespace Application.UseCases.System.CreateSystem;

public sealed record CreateSystemCommand(string Name, string Description, Ulid Id = default) : BaseCommand, IRequest<OperationResult>;