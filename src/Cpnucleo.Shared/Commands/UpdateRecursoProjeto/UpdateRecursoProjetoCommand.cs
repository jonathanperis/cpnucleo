namespace Cpnucleo.Shared.Commands.UpdateRecursoProjeto;

public sealed record UpdateRecursoProjetoCommand(Guid Id, Guid IdRecurso, Guid IdProjeto) : BaseCommand, IRequest<OperationResult>;