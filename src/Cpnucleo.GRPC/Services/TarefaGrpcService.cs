using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class TarefaGrpcService : ServiceBase<ITarefaGrpcService>, ITarefaGrpcService
{
    private readonly IAsyncRequestHandler<CreateTarefaCommand, OperationResult> _createTarefaCommand;
    private readonly IAsyncRequestHandler<ListTarefaQuery, IEnumerable<TarefaViewModel>> _listTarefaQuery;
    private readonly IAsyncRequestHandler<GetTarefaQuery, TarefaViewModel> _getTarefaQuery;
    private readonly IAsyncRequestHandler<GetByRecursoQuery, IEnumerable<TarefaViewModel>> _getByRecursoQuery;
    private readonly IAsyncRequestHandler<RemoveTarefaCommand, OperationResult> _removeTarefaCommand;
    private readonly IAsyncRequestHandler<UpdateTarefaCommand, OperationResult> _updateTarefaCommand;

    public TarefaGrpcService(IAsyncRequestHandler<CreateTarefaCommand, OperationResult> createTarefaCommand,
                             IAsyncRequestHandler<ListTarefaQuery, IEnumerable<TarefaViewModel>> listTarefaQuery,
                             IAsyncRequestHandler<GetTarefaQuery, TarefaViewModel> getTarefaQuery,
                             IAsyncRequestHandler<GetByRecursoQuery, IEnumerable<TarefaViewModel>> getByRecursoQuery,
                             IAsyncRequestHandler<RemoveTarefaCommand, OperationResult> removeTarefaCommand,
                             IAsyncRequestHandler<UpdateTarefaCommand, OperationResult> updateTarefaCommand)
    {
        _createTarefaCommand = createTarefaCommand;
        _listTarefaQuery = listTarefaQuery;
        _getTarefaQuery = getTarefaQuery;
        _getByRecursoQuery = getByRecursoQuery;
        _removeTarefaCommand = removeTarefaCommand;
        _updateTarefaCommand = updateTarefaCommand;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateTarefaCommand command)
    {
        return await _createTarefaCommand.InvokeAsync(command);
    }

    public async UnaryResult<IEnumerable<TarefaViewModel>> AllAsync(ListTarefaQuery query)
    {
        return await _listTarefaQuery.InvokeAsync(query);
    }

    public async UnaryResult<TarefaViewModel> GetAsync(GetTarefaQuery query)
    {
        return await _getTarefaQuery.InvokeAsync(query);
    }

    public async UnaryResult<IEnumerable<TarefaViewModel>> GetByRecursoAsync(GetByRecursoQuery query)
    {
        return await _getByRecursoQuery.InvokeAsync(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveTarefaCommand command)
    {
        return await _removeTarefaCommand.InvokeAsync(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateTarefaCommand command)
    {
        return await _updateTarefaCommand.InvokeAsync(command);
    }
}
