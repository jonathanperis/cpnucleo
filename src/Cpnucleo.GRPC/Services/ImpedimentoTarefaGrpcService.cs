using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class ImpedimentoTarefaGrpcService : ServiceBase<IImpedimentoTarefaGrpcService>, IImpedimentoTarefaGrpcService
{
    private readonly IAsyncRequestHandler<CreateImpedimentoTarefaCommand, OperationResult> _createImpedimentoTarefaCommand;
    private readonly IAsyncRequestHandler<ListImpedimentoTarefaQuery, IEnumerable<ImpedimentoTarefaViewModel>> _listImpedimentoTarefaQuery;
    private readonly IAsyncRequestHandler<GetImpedimentoTarefaQuery, ImpedimentoTarefaViewModel> _getImpedimentoTarefaQuery;
    private readonly IAsyncRequestHandler<GetByTarefaQuery, IEnumerable<ImpedimentoTarefaViewModel>> _getByTarefaQuery;
    private readonly IAsyncRequestHandler<RemoveImpedimentoTarefaCommand, OperationResult> _removeImpedimentoTarefaCommand;
    private readonly IAsyncRequestHandler<UpdateImpedimentoTarefaCommand, OperationResult> _updateImpedimentoTarefaCommand;

    public ImpedimentoTarefaGrpcService(IAsyncRequestHandler<CreateImpedimentoTarefaCommand, OperationResult> createImpedimentoTarefaCommand,
                                        IAsyncRequestHandler<ListImpedimentoTarefaQuery, IEnumerable<ImpedimentoTarefaViewModel>> listImpedimentoTarefaQuery,
                                        IAsyncRequestHandler<GetImpedimentoTarefaQuery, ImpedimentoTarefaViewModel> getImpedimentoTarefaQuery,
                                        IAsyncRequestHandler<GetByTarefaQuery, IEnumerable<ImpedimentoTarefaViewModel>> getByTarefaQuery,
                                        IAsyncRequestHandler<RemoveImpedimentoTarefaCommand, OperationResult> removeImpedimentoTarefaCommand,
                                        IAsyncRequestHandler<UpdateImpedimentoTarefaCommand, OperationResult> updateImpedimentoTarefaCommand)
    {
        _createImpedimentoTarefaCommand = createImpedimentoTarefaCommand;
        _listImpedimentoTarefaQuery = listImpedimentoTarefaQuery;
        _getImpedimentoTarefaQuery = getImpedimentoTarefaQuery;
        _getByTarefaQuery = getByTarefaQuery;
        _removeImpedimentoTarefaCommand = removeImpedimentoTarefaCommand;
        _updateImpedimentoTarefaCommand = updateImpedimentoTarefaCommand;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateImpedimentoTarefaCommand command)
    {
        return await _createImpedimentoTarefaCommand.InvokeAsync(command);
    }

    public async UnaryResult<IEnumerable<ImpedimentoTarefaViewModel>> AllAsync(ListImpedimentoTarefaQuery query)
    {
        return await _listImpedimentoTarefaQuery.InvokeAsync(query);
    }

    public async UnaryResult<ImpedimentoTarefaViewModel> GetAsync(GetImpedimentoTarefaQuery query)
    {
        return await _getImpedimentoTarefaQuery.InvokeAsync(query);
    }

    public async UnaryResult<IEnumerable<ImpedimentoTarefaViewModel>> GetByTarefaAsync(GetByTarefaQuery query)
    {
        return await _getByTarefaQuery.InvokeAsync(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveImpedimentoTarefaCommand command)
    {
        return await _removeImpedimentoTarefaCommand.InvokeAsync(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateImpedimentoTarefaCommand command)
    {
        return await _updateImpedimentoTarefaCommand.InvokeAsync(command);
    }
}
