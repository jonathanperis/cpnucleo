namespace Cpnucleo.Shared.Commands.RemoveRecursoProjeto;

public sealed record RemoveRecursoProjetoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;