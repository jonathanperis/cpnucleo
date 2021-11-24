using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

public interface ITarefaGrpcService : IService<ITarefaGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateTarefaCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateTarefaCommand command);

    UnaryResult<TarefaViewModel> GetAsync(GetTarefaQuery query);

    UnaryResult<IEnumerable<TarefaViewModel>> AllAsync(ListTarefaQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveTarefaCommand command);

    UnaryResult<IEnumerable<TarefaViewModel>> GetByRecursoAsync(GetByRecursoQuery query);
}