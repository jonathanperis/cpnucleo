using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class RecursoProjetoGrpcService : ServiceBase<IRecursoProjetoGrpcService>, IRecursoProjetoGrpcService
{
    private readonly IAsyncRequestHandler<CreateRecursoProjetoCommand, OperationResult> _createRecursoProjetoCommand;
    private readonly IAsyncRequestHandler<ListRecursoProjetoQuery, IEnumerable<RecursoProjetoViewModel>> _listRecursoProjetoQuery;
    private readonly IAsyncRequestHandler<GetRecursoProjetoQuery, RecursoProjetoViewModel> _getRecursoProjetoQuery;
    private readonly IAsyncRequestHandler<GetByProjetoQuery, IEnumerable<RecursoProjetoViewModel>> _getByProjetoQuery;
    private readonly IAsyncRequestHandler<RemoveRecursoProjetoCommand, OperationResult> _removeRecursoProjetoCommand;
    private readonly IAsyncRequestHandler<UpdateRecursoProjetoCommand, OperationResult> _updateRecursoProjetoCommand;

    public RecursoProjetoGrpcService(IAsyncRequestHandler<CreateRecursoProjetoCommand, OperationResult> createRecursoProjetoCommand,
                                     IAsyncRequestHandler<ListRecursoProjetoQuery, IEnumerable<RecursoProjetoViewModel>> listRecursoProjetoQuery,
                                     IAsyncRequestHandler<GetRecursoProjetoQuery, RecursoProjetoViewModel> getRecursoProjetoQuery,
                                     IAsyncRequestHandler<GetByProjetoQuery, IEnumerable<RecursoProjetoViewModel>> getByProjetoQuery,
                                     IAsyncRequestHandler<RemoveRecursoProjetoCommand, OperationResult> removeRecursoProjetoCommand,
                                     IAsyncRequestHandler<UpdateRecursoProjetoCommand, OperationResult> updateRecursoProjetoCommand)
    {
        _createRecursoProjetoCommand = createRecursoProjetoCommand;
        _listRecursoProjetoQuery = listRecursoProjetoQuery;
        _getRecursoProjetoQuery = getRecursoProjetoQuery;
        _getByProjetoQuery = getByProjetoQuery;
        _removeRecursoProjetoCommand = removeRecursoProjetoCommand;
        _updateRecursoProjetoCommand = updateRecursoProjetoCommand;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateRecursoProjetoCommand command)
    {
        return await _createRecursoProjetoCommand.InvokeAsync(command);
    }

    public async UnaryResult<IEnumerable<RecursoProjetoViewModel>> AllAsync(ListRecursoProjetoQuery query)
    {
        return await _listRecursoProjetoQuery.InvokeAsync(query);
    }

    public async UnaryResult<RecursoProjetoViewModel> GetAsync(GetRecursoProjetoQuery query)
    {
        return await _getRecursoProjetoQuery.InvokeAsync(query);
    }

    public async UnaryResult<IEnumerable<RecursoProjetoViewModel>> GetByProjetoAsync(GetByProjetoQuery query)
    {
        return await _getByProjetoQuery.InvokeAsync(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveRecursoProjetoCommand command)
    {
        return await _removeRecursoProjetoCommand.InvokeAsync(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateRecursoProjetoCommand command)
    {
        return await _updateRecursoProjetoCommand.InvokeAsync(command);
    }
}
