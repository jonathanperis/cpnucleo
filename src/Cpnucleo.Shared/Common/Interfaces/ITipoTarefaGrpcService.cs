using Cpnucleo.Shared.Commands.TipoTarefa;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.TipoTarefa;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface ITipoTarefaGrpcService : IService<ITipoTarefaGrpcService>
{
    UnaryResult<OperationResult> CreateTipoTarefa(CreateTipoTarefaCommand command);

    UnaryResult<OperationResult> UpdateTipoTarefa(UpdateTipoTarefaCommand command);

    UnaryResult<GetTipoTarefaViewModel> GetTipoTarefa(GetTipoTarefaQuery query);

    UnaryResult<ListTipoTarefaViewModel> ListTipoTarefa(ListTipoTarefaQuery query);

    UnaryResult<OperationResult> RemoveTipoTarefa(RemoveTipoTarefaCommand command);
}