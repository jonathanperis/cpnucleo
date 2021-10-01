using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.CreateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.RemoveSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.UpdateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.GetSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class SistemaGrpcService : ServiceBase<ISistemaGrpcService>, ISistemaGrpcService
{
    private readonly IAsyncRequestHandler<CreateSistemaCommand, CreateSistemaResponse> _createSistemaCommand;
    private readonly IAsyncRequestHandler<ListSistemaQuery, ListSistemaResponse> _listSistemaQuery;
    private readonly IAsyncRequestHandler<GetSistemaQuery, GetSistemaResponse> _getSistemaQuery;
    private readonly IAsyncRequestHandler<RemoveSistemaCommand, RemoveSistemaResponse> _removeSistemaCommand;
    private readonly IAsyncRequestHandler<UpdateSistemaCommand, UpdateSistemaResponse> _updateSistemaCommand;

    public SistemaGrpcService(IAsyncRequestHandler<CreateSistemaCommand, CreateSistemaResponse> createSistemaCommand,
                              IAsyncRequestHandler<ListSistemaQuery, ListSistemaResponse> listSistemaQuery,
                              IAsyncRequestHandler<GetSistemaQuery, GetSistemaResponse> getSistemaQuery,
                              IAsyncRequestHandler<RemoveSistemaCommand, RemoveSistemaResponse> removeSistemaCommand,
                              IAsyncRequestHandler<UpdateSistemaCommand, UpdateSistemaResponse> updateSistemaCommand)
    {
        _createSistemaCommand = createSistemaCommand;
        _listSistemaQuery = listSistemaQuery;
        _getSistemaQuery = getSistemaQuery;
        _removeSistemaCommand = removeSistemaCommand;
        _updateSistemaCommand = updateSistemaCommand;
    }

    public async UnaryResult<CreateSistemaResponse> AddAsync(CreateSistemaCommand command)
    {
        return await _createSistemaCommand.InvokeAsync(command);
    }

    public async UnaryResult<ListSistemaResponse> AllAsync(ListSistemaQuery query)
    {
        return await _listSistemaQuery.InvokeAsync(query);
    }

    public async UnaryResult<GetSistemaResponse> GetAsync(GetSistemaQuery query)
    {
        return await _getSistemaQuery.InvokeAsync(query);
    }

    public async UnaryResult<RemoveSistemaResponse> RemoveAsync(RemoveSistemaCommand command)
    {
        return await _removeSistemaCommand.InvokeAsync(command);
    }

    public async UnaryResult<UpdateSistemaResponse> UpdateAsync(UpdateSistemaCommand command)
    {
        return await _updateSistemaCommand.InvokeAsync(command);
    }
}
