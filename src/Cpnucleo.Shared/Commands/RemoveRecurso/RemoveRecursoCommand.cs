namespace Cpnucleo.Shared.Commands.RemoveRecurso;

public sealed record RemoveRecursoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;