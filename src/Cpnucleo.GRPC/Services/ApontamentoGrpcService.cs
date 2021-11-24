using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class ApontamentoGrpcService : ServiceBase<IApontamentoGrpcService>, IApontamentoGrpcService
{
    private readonly IAsyncRequestHandler<CreateApontamentoCommand, OperationResult> _createApontamentoCommand;
    private readonly IAsyncRequestHandler<ListApontamentoQuery, IEnumerable<ApontamentoViewModel>> _listApontamentoQuery;
    private readonly IAsyncRequestHandler<GetApontamentoQuery, ApontamentoViewModel> _getApontamentoQuery;
    private readonly IAsyncRequestHandler<GetByRecursoQuery, IEnumerable<ApontamentoViewModel>> _getByRecursoQuery;
    private readonly IAsyncRequestHandler<RemoveApontamentoCommand, OperationResult> _removeApontamentoCommand;
    private readonly IAsyncRequestHandler<UpdateApontamentoCommand, OperationResult> _updateApontamentoCommand;

    public ApontamentoGrpcService(IAsyncRequestHandler<CreateApontamentoCommand, OperationResult> createApontamentoCommand,
                                  IAsyncRequestHandler<ListApontamentoQuery, IEnumerable<ApontamentoViewModel>> listApontamentoQuery,
                                  IAsyncRequestHandler<GetApontamentoQuery, ApontamentoViewModel> getApontamentoQuery,
                                  IAsyncRequestHandler<GetByRecursoQuery, IEnumerable<ApontamentoViewModel>> getByRecursoQuery,
                                  IAsyncRequestHandler<RemoveApontamentoCommand, OperationResult> removeApontamentoCommand,
                                  IAsyncRequestHandler<UpdateApontamentoCommand, OperationResult> updateApontamentoCommand)
    {
        _createApontamentoCommand = createApontamentoCommand;
        _listApontamentoQuery = listApontamentoQuery;
        _getApontamentoQuery = getApontamentoQuery;
        _getByRecursoQuery = getByRecursoQuery;
        _removeApontamentoCommand = removeApontamentoCommand;
        _updateApontamentoCommand = updateApontamentoCommand;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateApontamentoCommand command)
    {
        return await _createApontamentoCommand.InvokeAsync(command);
    }

    public async UnaryResult<IEnumerable<ApontamentoViewModel>> AllAsync(ListApontamentoQuery query)
    {
        return await _listApontamentoQuery.InvokeAsync(query);
    }

    public async UnaryResult<ApontamentoViewModel> GetAsync(GetApontamentoQuery query)
    {
        return await _getApontamentoQuery.InvokeAsync(query);
    }

    public async UnaryResult<IEnumerable<ApontamentoViewModel>> GetByRecursoAsync(GetByRecursoQuery query)
    {
        return await _getByRecursoQuery.InvokeAsync(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveApontamentoCommand command)
    {
        return await _removeApontamentoCommand.InvokeAsync(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateApontamentoCommand command)
    {
        return await _updateApontamentoCommand.InvokeAsync(command);
    }
}
