namespace Cpnucleo.Shared.Commands.RecursoProjeto;

public sealed record RemoveRecursoProjetoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;