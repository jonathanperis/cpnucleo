namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.RecursoProjeto;

public record CreateRecursoProjetoCommand(Guid Id, Guid IdRecurso, Guid IdProjeto) : BaseCommand, IRequest<OperationResult>;