namespace Cpnucleo.Shared.Commands.CreateTarefa;

public sealed record CreateTarefaCommand(string Nome, DateTime DataInicio, DateTime DataTermino, int QtdHoras, string Detalhe, Guid IdProjeto, Guid IdWorkflow, Guid IdRecurso, Guid IdTipoTarefa) : BaseCommand, IRequest<OperationResult>;