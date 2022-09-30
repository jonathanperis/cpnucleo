namespace Cpnucleo.Shared.Commands.Tarefa;

public sealed record UpdateTarefaCommand(Guid Id, string Nome, DateTime DataInicio, DateTime DataTermino, int QtdHoras, string Detalhe, Guid IdProjeto, Guid IdWorkflow, Guid IdRecurso, Guid IdTipoTarefa) : BaseCommand, IRequest<OperationResult>;