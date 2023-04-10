namespace Cpnucleo.Shared.Commands.CreateRecursoProjeto;

public sealed record CreateRecursoProjetoCommand(Guid IdRecurso, Guid IdProjeto, Guid Id = default) : BaseCommand, IRequest<OperationResult>;