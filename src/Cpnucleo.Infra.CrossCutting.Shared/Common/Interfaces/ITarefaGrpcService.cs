namespace Cpnucleo.Infra.CrossCutting.Shared.Common.Interfaces;

public interface ITarefaGrpcService : IService<ITarefaGrpcService>
{
    UnaryResult<OperationResult> CreateTarefa(CreateTarefaCommand command);

    UnaryResult<OperationResult> UpdateTarefa(UpdateTarefaCommand command);

    UnaryResult<GetTarefaViewModel> GetTarefa(GetTarefaQuery query);

    UnaryResult<ListTarefaViewModel> ListTarefa(ListTarefaQuery query);

    UnaryResult<OperationResult> RemoveTarefa(RemoveTarefaCommand command);

    UnaryResult<GetTarefaByRecursoViewModel> GetTarefaByRecurso(GetTarefaByRecursoQuery query);

    UnaryResult<OperationResult> UpdateTarefaByWorkflow(UpdateTarefaByWorkflowCommand command);
}