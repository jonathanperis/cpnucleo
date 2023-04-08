using Cpnucleo.Shared.Commands.CreateTipoTarefa;
using Cpnucleo.Shared.Commands.RemoveTipoTarefa;
using Cpnucleo.Shared.Commands.UpdateTipoTarefa;
using Cpnucleo.Shared.Queries.GetTipoTarefa;
using Cpnucleo.Shared.Queries.ListTipoTarefa;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface ITipoTarefaGrpcService : IService<ITipoTarefaGrpcService>
{
    UnaryResult<OperationResult> CreateTipoTarefa(CreateTipoTarefaCommand command);

    UnaryResult<OperationResult> UpdateTipoTarefa(UpdateTipoTarefaCommand command);

    UnaryResult<GetTipoTarefaViewModel> GetTipoTarefa(GetTipoTarefaQuery query);

    UnaryResult<ListTipoTarefaViewModel> ListTipoTarefa(ListTipoTarefaQuery query);

    UnaryResult<OperationResult> RemoveTipoTarefa(RemoveTipoTarefaCommand command);
}