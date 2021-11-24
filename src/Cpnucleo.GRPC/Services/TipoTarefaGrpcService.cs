using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class TipoTarefaGrpcService : ServiceBase<ITipoTarefaGrpcService>, ITipoTarefaGrpcService
{
    private readonly IAsyncRequestHandler<CreateTipoTarefaCommand, OperationResult> _createTipoTarefaCommand;
    private readonly IAsyncRequestHandler<ListTipoTarefaQuery, IEnumerable<TipoTarefaViewModel>> _listTipoTarefaQuery;
    private readonly IAsyncRequestHandler<GetTipoTarefaQuery, TipoTarefaViewModel> _getTipoTarefaQuery;
    private readonly IAsyncRequestHandler<RemoveTipoTarefaCommand, OperationResult> _removeTipoTarefaCommand;
    private readonly IAsyncRequestHandler<UpdateTipoTarefaCommand, OperationResult> _updateTipoTarefaCommand;

    public TipoTarefaGrpcService(IAsyncRequestHandler<CreateTipoTarefaCommand, OperationResult> createTipoTarefaCommand,
                                 IAsyncRequestHandler<ListTipoTarefaQuery, IEnumerable<TipoTarefaViewModel>> listTipoTarefaQuery,
                                 IAsyncRequestHandler<GetTipoTarefaQuery, TipoTarefaViewModel> getTipoTarefaQuery,
                                 IAsyncRequestHandler<RemoveTipoTarefaCommand, OperationResult> removeTipoTarefaCommand,
                                 IAsyncRequestHandler<UpdateTipoTarefaCommand, OperationResult> updateTipoTarefaCommand)
    {
        _createTipoTarefaCommand = createTipoTarefaCommand;
        _listTipoTarefaQuery = listTipoTarefaQuery;
        _getTipoTarefaQuery = getTipoTarefaQuery;
        _removeTipoTarefaCommand = removeTipoTarefaCommand;
        _updateTipoTarefaCommand = updateTipoTarefaCommand;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateTipoTarefaCommand command)
    {
        return await _createTipoTarefaCommand.InvokeAsync(command);
    }

    public async UnaryResult<IEnumerable<TipoTarefaViewModel>> AllAsync(ListTipoTarefaQuery query)
    {
        return await _listTipoTarefaQuery.InvokeAsync(query);
    }

    public async UnaryResult<TipoTarefaViewModel> GetAsync(GetTipoTarefaQuery query)
    {
        return await _getTipoTarefaQuery.InvokeAsync(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveTipoTarefaCommand command)
    {
        return await _removeTipoTarefaCommand.InvokeAsync(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateTipoTarefaCommand command)
    {
        return await _updateTipoTarefaCommand.InvokeAsync(command);
    }
}
