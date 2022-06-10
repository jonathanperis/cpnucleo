namespace Cpnucleo.Infra.CrossCutting.Shared.Common.Interfaces;

public interface IRecursoTarefaGrpcService : IService<IRecursoTarefaGrpcService>
{
    UnaryResult<OperationResult> CreateRecursoTarefa(CreateRecursoTarefaCommand command);

    UnaryResult<OperationResult> UpdateRecursoTarefa(UpdateRecursoTarefaCommand command);

    UnaryResult<GetRecursoTarefaViewModel> GetRecursoTarefa(GetRecursoTarefaQuery query);

    UnaryResult<ListRecursoTarefaViewModel> ListRecursoTarefa(ListRecursoTarefaQuery query);

    UnaryResult<OperationResult> RemoveRecursoTarefa(RemoveRecursoTarefaCommand command);

    UnaryResult<GetRecursoTarefaByTarefaViewModel> GetRecursoTarefaByTarefa(GetRecursoTarefaByTarefaQuery query);
}