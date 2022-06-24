namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.RecursoProjeto;

public record UpdateRecursoProjetoCommand(Guid Id, Guid IdRecurso, Guid IdProjeto) : BaseCommand, IRequest<OperationResult>;