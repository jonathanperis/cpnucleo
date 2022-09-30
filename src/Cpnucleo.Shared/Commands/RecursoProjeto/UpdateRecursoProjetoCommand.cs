namespace Cpnucleo.Shared.Commands.RecursoProjeto;

public sealed record UpdateRecursoProjetoCommand(Guid Id, Guid IdRecurso, Guid IdProjeto) : BaseCommand, IRequest<OperationResult>;