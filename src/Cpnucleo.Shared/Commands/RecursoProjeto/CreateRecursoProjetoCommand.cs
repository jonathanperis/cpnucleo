namespace Cpnucleo.Shared.Commands.RecursoProjeto;

public sealed record CreateRecursoProjetoCommand(Guid Id, Guid IdRecurso, Guid IdProjeto) : BaseCommand, IRequest<OperationResult>;