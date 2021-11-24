using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

public interface IRecursoTarefaGrpcService : IService<IRecursoTarefaGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateRecursoTarefaCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateRecursoTarefaCommand command);

    UnaryResult<RecursoTarefaViewModel> GetAsync(GetRecursoTarefaQuery query);

    UnaryResult<IEnumerable<RecursoTarefaViewModel>> AllAsync(ListRecursoTarefaQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveRecursoTarefaCommand command);

    UnaryResult<IEnumerable<RecursoTarefaViewModel>> GetByTarefaAsync(GetByTarefaQuery query);
}