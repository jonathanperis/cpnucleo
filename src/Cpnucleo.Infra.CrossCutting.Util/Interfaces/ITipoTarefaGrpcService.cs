namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.CreateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.RemoveTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.UpdateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.GetTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.ListTipoTarefa;

public interface ITipoTarefaGrpcService : IService<ITipoTarefaGrpcService>
{
    UnaryResult<CreateTipoTarefaResponse> AddAsync(CreateTipoTarefaCommand command);

    UnaryResult<UpdateTipoTarefaResponse> UpdateAsync(UpdateTipoTarefaCommand command);

    UnaryResult<GetTipoTarefaResponse> GetAsync(GetTipoTarefaQuery query);

    UnaryResult<ListTipoTarefaResponse> AllAsync(ListTipoTarefaQuery query);

    UnaryResult<RemoveTipoTarefaResponse> RemoveAsync(RemoveTipoTarefaCommand command);
}