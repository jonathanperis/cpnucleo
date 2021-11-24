using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

public interface ITipoTarefaGrpcService : IService<ITipoTarefaGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateTipoTarefaCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateTipoTarefaCommand command);

    UnaryResult<TipoTarefaViewModel> GetAsync(GetTipoTarefaQuery query);

    UnaryResult<IEnumerable<TipoTarefaViewModel>> AllAsync(ListTipoTarefaQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveTipoTarefaCommand command);
}