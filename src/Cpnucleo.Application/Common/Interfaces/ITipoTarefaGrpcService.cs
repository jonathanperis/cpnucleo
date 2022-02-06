using Cpnucleo.Application.Commands.TipoTarefa.CreateTipoTarefa;
using Cpnucleo.Application.Commands.TipoTarefa.RemoveTipoTarefa;
using Cpnucleo.Application.Commands.TipoTarefa.UpdateTipoTarefa;
using Cpnucleo.Application.Queries.TipoTarefa.GetTipoTarefa;
using Cpnucleo.Application.Queries.TipoTarefa.ListTipoTarefa;
using MagicOnion;

namespace Cpnucleo.Application.Common.Interfaces;

public interface ITipoTarefaGrpcService : IService<ITipoTarefaGrpcService>
{
    UnaryResult<OperationResult> CreateTipoTarefa(CreateTipoTarefaCommand command);

    UnaryResult<OperationResult> UpdateTipoTarefa(UpdateTipoTarefaCommand command);

    UnaryResult<GetTipoTarefaViewModel> GetTipoTarefa(GetTipoTarefaQuery query);

    UnaryResult<ListTipoTarefaViewModel> ListTipoTarefa(ListTipoTarefaQuery query);

    UnaryResult<OperationResult> RemoveTipoTarefa(RemoveTipoTarefaCommand command);
}