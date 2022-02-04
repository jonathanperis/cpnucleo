using Cpnucleo.Application.Commands.TipoTarefa.CreateTipoTarefa;
using Cpnucleo.Application.Commands.TipoTarefa.RemoveTipoTarefa;
using Cpnucleo.Application.Commands.TipoTarefa.UpdateTipoTarefa;
using Cpnucleo.Application.Queries.TipoTarefa.GetTipoTarefa;
using Cpnucleo.Application.Queries.TipoTarefa.ListTipoTarefa;

namespace Cpnucleo.Application.Interfaces;

public interface ITipoTarefaGrpcService : IService<ITipoTarefaGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateTipoTarefaCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateTipoTarefaCommand command);

    UnaryResult<GetTipoTarefaViewModel> GetAsync(GetTipoTarefaQuery query);

    UnaryResult<ListTipoTarefaViewModel> AllAsync(ListTipoTarefaQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveTipoTarefaCommand command);
}