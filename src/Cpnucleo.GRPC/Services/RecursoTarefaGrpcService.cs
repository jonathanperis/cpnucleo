using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class RecursoTarefaGrpcService : ServiceBase<IRecursoTarefaGrpcService>, IRecursoTarefaGrpcService
{
    private readonly IAsyncRequestHandler<CreateRecursoTarefaCommand, OperationResult> _createRecursoTarefaCommand;
    private readonly IAsyncRequestHandler<ListRecursoTarefaQuery, IEnumerable<RecursoTarefaViewModel>> _listRecursoTarefaQuery;
    private readonly IAsyncRequestHandler<GetRecursoTarefaQuery, RecursoTarefaViewModel> _getRecursoTarefaQuery;
    private readonly IAsyncRequestHandler<GetByTarefaQuery, IEnumerable<RecursoTarefaViewModel>> _getByTarefaQuery;
    private readonly IAsyncRequestHandler<RemoveRecursoTarefaCommand, OperationResult> _removeRecursoTarefaCommand;
    private readonly IAsyncRequestHandler<UpdateRecursoTarefaCommand, OperationResult> _updateRecursoTarefaCommand;

    public RecursoTarefaGrpcService(IAsyncRequestHandler<CreateRecursoTarefaCommand, OperationResult> createRecursoTarefaCommand,
                                    IAsyncRequestHandler<ListRecursoTarefaQuery, IEnumerable<RecursoTarefaViewModel>> listRecursoTarefaQuery,
                                    IAsyncRequestHandler<GetRecursoTarefaQuery, RecursoTarefaViewModel> getRecursoTarefaQuery,
                                    IAsyncRequestHandler<GetByTarefaQuery, IEnumerable<RecursoTarefaViewModel>> getByTarefaQuery,
                                    IAsyncRequestHandler<RemoveRecursoTarefaCommand, OperationResult> removeRecursoTarefaCommand,
                                    IAsyncRequestHandler<UpdateRecursoTarefaCommand, OperationResult> updateRecursoTarefaCommand)
    {
        _createRecursoTarefaCommand = createRecursoTarefaCommand;
        _listRecursoTarefaQuery = listRecursoTarefaQuery;
        _getRecursoTarefaQuery = getRecursoTarefaQuery;
        _getByTarefaQuery = getByTarefaQuery;
        _removeRecursoTarefaCommand = removeRecursoTarefaCommand;
        _updateRecursoTarefaCommand = updateRecursoTarefaCommand;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateRecursoTarefaCommand command)
    {
        return await _createRecursoTarefaCommand.InvokeAsync(command);
    }

    public async UnaryResult<IEnumerable<RecursoTarefaViewModel>> AllAsync(ListRecursoTarefaQuery query)
    {
        return await _listRecursoTarefaQuery.InvokeAsync(query);
    }

    public async UnaryResult<RecursoTarefaViewModel> GetAsync(GetRecursoTarefaQuery query)
    {
        return await _getRecursoTarefaQuery.InvokeAsync(query);
    }

    public async UnaryResult<IEnumerable<RecursoTarefaViewModel>> GetByTarefaAsync(GetByTarefaQuery query)
    {
        return await _getByTarefaQuery.InvokeAsync(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveRecursoTarefaCommand command)
    {
        return await _removeRecursoTarefaCommand.InvokeAsync(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateRecursoTarefaCommand command)
    {
        return await _updateRecursoTarefaCommand.InvokeAsync(command);
    }
}
