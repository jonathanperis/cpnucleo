using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class ImpedimentoGrpcService : ServiceBase<IImpedimentoGrpcService>, IImpedimentoGrpcService
{
    private readonly IAsyncRequestHandler<CreateImpedimentoCommand, OperationResult> _createImpedimentoCommand;
    private readonly IAsyncRequestHandler<ListImpedimentoQuery, IEnumerable<ImpedimentoViewModel>> _listImpedimentoQuery;
    private readonly IAsyncRequestHandler<GetImpedimentoQuery, ImpedimentoViewModel> _getImpedimentoQuery;
    private readonly IAsyncRequestHandler<RemoveImpedimentoCommand, OperationResult> _removeImpedimentoCommand;
    private readonly IAsyncRequestHandler<UpdateImpedimentoCommand, OperationResult> _updateImpedimentoCommand;

    public ImpedimentoGrpcService(IAsyncRequestHandler<CreateImpedimentoCommand, OperationResult> createImpedimentoCommand,
                                  IAsyncRequestHandler<ListImpedimentoQuery, IEnumerable<ImpedimentoViewModel>> listImpedimentoQuery,
                                  IAsyncRequestHandler<GetImpedimentoQuery, ImpedimentoViewModel> getImpedimentoQuery,
                                  IAsyncRequestHandler<RemoveImpedimentoCommand, OperationResult> removeImpedimentoCommand,
                                  IAsyncRequestHandler<UpdateImpedimentoCommand, OperationResult> updateImpedimentoCommand)
    {
        _createImpedimentoCommand = createImpedimentoCommand;
        _listImpedimentoQuery = listImpedimentoQuery;
        _getImpedimentoQuery = getImpedimentoQuery;
        _removeImpedimentoCommand = removeImpedimentoCommand;
        _updateImpedimentoCommand = updateImpedimentoCommand;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateImpedimentoCommand command)
    {
        return await _createImpedimentoCommand.InvokeAsync(command);
    }

    public async UnaryResult<IEnumerable<ImpedimentoViewModel>> AllAsync(ListImpedimentoQuery query)
    {
        return await _listImpedimentoQuery.InvokeAsync(query);
    }

    public async UnaryResult<ImpedimentoViewModel> GetAsync(GetImpedimentoQuery query)
    {
        return await _getImpedimentoQuery.InvokeAsync(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveImpedimentoCommand command)
    {
        return await _removeImpedimentoCommand.InvokeAsync(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateImpedimentoCommand command)
    {
        return await _updateImpedimentoCommand.InvokeAsync(command);
    }
}
