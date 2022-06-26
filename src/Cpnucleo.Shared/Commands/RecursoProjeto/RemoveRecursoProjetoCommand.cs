namespace Cpnucleo.Shared.Commands.RecursoProjeto;

public record RemoveRecursoProjetoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;