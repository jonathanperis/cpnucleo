namespace Cpnucleo.Shared.Commands.CreateRecursoProjeto;

public sealed record CreateRecursoProjetoCommand(Guid IdRecurso, Guid IdProjeto) : BaseCommand, IRequest<OperationResult>;