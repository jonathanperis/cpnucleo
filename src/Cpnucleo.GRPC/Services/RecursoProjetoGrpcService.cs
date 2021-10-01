using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.CreateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.RemoveRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.UpdateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetByProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.ListRecursoProjeto;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class RecursoProjetoGrpcService : ServiceBase<IRecursoProjetoGrpcService>, IRecursoProjetoGrpcService
{
    private readonly IAsyncRequestHandler<CreateRecursoProjetoCommand, CreateRecursoProjetoResponse> _createRecursoProjetoCommand;
    private readonly IAsyncRequestHandler<ListRecursoProjetoQuery, ListRecursoProjetoResponse> _listRecursoProjetoQuery;
    private readonly IAsyncRequestHandler<GetRecursoProjetoQuery, GetRecursoProjetoResponse> _getRecursoProjetoQuery;
    private readonly IAsyncRequestHandler<GetByProjetoQuery, GetByProjetoResponse> _getByProjetoQuery;
    private readonly IAsyncRequestHandler<RemoveRecursoProjetoCommand, RemoveRecursoProjetoResponse> _removeRecursoProjetoCommand;
    private readonly IAsyncRequestHandler<UpdateRecursoProjetoCommand, UpdateRecursoProjetoResponse> _updateRecursoProjetoCommand;

    public RecursoProjetoGrpcService(IAsyncRequestHandler<CreateRecursoProjetoCommand, CreateRecursoProjetoResponse> createRecursoProjetoCommand,
                                     IAsyncRequestHandler<ListRecursoProjetoQuery, ListRecursoProjetoResponse> listRecursoProjetoQuery,
                                     IAsyncRequestHandler<GetRecursoProjetoQuery, GetRecursoProjetoResponse> getRecursoProjetoQuery,
                                     IAsyncRequestHandler<GetByProjetoQuery, GetByProjetoResponse> getByProjetoQuery,
                                     IAsyncRequestHandler<RemoveRecursoProjetoCommand, RemoveRecursoProjetoResponse> removeRecursoProjetoCommand,
                                     IAsyncRequestHandler<UpdateRecursoProjetoCommand, UpdateRecursoProjetoResponse> updateRecursoProjetoCommand)
    {
        _createRecursoProjetoCommand = createRecursoProjetoCommand;
        _listRecursoProjetoQuery = listRecursoProjetoQuery;
        _getRecursoProjetoQuery = getRecursoProjetoQuery;
        _getByProjetoQuery = getByProjetoQuery;
        _removeRecursoProjetoCommand = removeRecursoProjetoCommand;
        _updateRecursoProjetoCommand = updateRecursoProjetoCommand;
    }

    public async UnaryResult<CreateRecursoProjetoResponse> AddAsync(CreateRecursoProjetoCommand command)
    {
        return await _createRecursoProjetoCommand.InvokeAsync(command);
    }

    public async UnaryResult<ListRecursoProjetoResponse> AllAsync(ListRecursoProjetoQuery query)
    {
        return await _listRecursoProjetoQuery.InvokeAsync(query);
    }

    public async UnaryResult<GetRecursoProjetoResponse> GetAsync(GetRecursoProjetoQuery query)
    {
        return await _getRecursoProjetoQuery.InvokeAsync(query);
    }

    public async UnaryResult<GetByProjetoResponse> GetByProjetoAsync(GetByProjetoQuery query)
    {
        return await _getByProjetoQuery.InvokeAsync(query);
    }

    public async UnaryResult<RemoveRecursoProjetoResponse> RemoveAsync(RemoveRecursoProjetoCommand command)
    {
        return await _removeRecursoProjetoCommand.InvokeAsync(command);
    }

    public async UnaryResult<UpdateRecursoProjetoResponse> UpdateAsync(UpdateRecursoProjetoCommand command)
    {
        return await _updateRecursoProjetoCommand.InvokeAsync(command);
    }
}
