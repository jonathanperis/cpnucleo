namespace Cpnucleo.Shared.Commands.CreateRecursoProjeto;

public sealed record CreateRecursoProjetoCommand(Guid Id, Guid IdRecurso, Guid IdProjeto) : BaseCommand, IRequest<OperationResult>;