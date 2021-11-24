using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class SistemaGrpcService : ServiceBase<ISistemaGrpcService>, ISistemaGrpcService
{
    private readonly IAsyncRequestHandler<CreateSistemaCommand, OperationResult> _createSistemaCommand;
    private readonly IAsyncRequestHandler<ListSistemaQuery, IEnumerable<SistemaViewModel>> _listSistemaQuery;
    private readonly IAsyncRequestHandler<GetSistemaQuery, SistemaViewModel> _getSistemaQuery;
    private readonly IAsyncRequestHandler<RemoveSistemaCommand, OperationResult> _removeSistemaCommand;
    private readonly IAsyncRequestHandler<UpdateSistemaCommand, OperationResult> _updateSistemaCommand;

    public SistemaGrpcService(IAsyncRequestHandler<CreateSistemaCommand, OperationResult> createSistemaCommand,
                              IAsyncRequestHandler<ListSistemaQuery, IEnumerable<SistemaViewModel>> listSistemaQuery,
                              IAsyncRequestHandler<GetSistemaQuery, SistemaViewModel> getSistemaQuery,
                              IAsyncRequestHandler<RemoveSistemaCommand, OperationResult> removeSistemaCommand,
                              IAsyncRequestHandler<UpdateSistemaCommand, OperationResult> updateSistemaCommand)
    {
        _createSistemaCommand = createSistemaCommand;
        _listSistemaQuery = listSistemaQuery;
        _getSistemaQuery = getSistemaQuery;
        _removeSistemaCommand = removeSistemaCommand;
        _updateSistemaCommand = updateSistemaCommand;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateSistemaCommand command)
    {
        return await _createSistemaCommand.InvokeAsync(command);
    }

    public async UnaryResult<IEnumerable<SistemaViewModel>> AllAsync(ListSistemaQuery query)
    {
        return await _listSistemaQuery.InvokeAsync(query);
    }

    public async UnaryResult<SistemaViewModel> GetAsync(GetSistemaQuery query)
    {
        return await _getSistemaQuery.InvokeAsync(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveSistemaCommand command)
    {
        return await _removeSistemaCommand.InvokeAsync(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateSistemaCommand command)
    {
        return await _updateSistemaCommand.InvokeAsync(command);
    }
}
