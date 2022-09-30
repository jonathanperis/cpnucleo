namespace Cpnucleo.Shared.Commands.Recurso;

public sealed record RemoveRecursoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;