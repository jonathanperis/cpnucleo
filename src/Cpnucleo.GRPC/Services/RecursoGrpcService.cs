using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.Auth;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Services;

public class RecursoGrpcService : ServiceBase<IRecursoGrpcService>, IRecursoGrpcService
{
    private readonly IAsyncRequestHandler<CreateRecursoCommand, OperationResult> _createRecursoCommand;
    private readonly IAsyncRequestHandler<ListRecursoQuery, IEnumerable<RecursoViewModel>> _listRecursoQuery;
    private readonly IAsyncRequestHandler<AuthQuery, AuthResponse> _authQuery;
    private readonly IAsyncRequestHandler<GetRecursoQuery, RecursoViewModel> _getRecursoQuery;
    private readonly IAsyncRequestHandler<RemoveRecursoCommand, OperationResult> _removeRecursoCommand;
    private readonly IAsyncRequestHandler<UpdateRecursoCommand, OperationResult> _updateRecursoCommand;
    private readonly IConfiguration _configuration;

    public RecursoGrpcService(IAsyncRequestHandler<CreateRecursoCommand, OperationResult> createRecursoCommand,
                              IAsyncRequestHandler<ListRecursoQuery, IEnumerable<RecursoViewModel>> listRecursoQuery,
                              IAsyncRequestHandler<AuthQuery, AuthResponse> authQuery,
                              IAsyncRequestHandler<GetRecursoQuery, RecursoViewModel> getRecursoQuery,
                              IAsyncRequestHandler<RemoveRecursoCommand, OperationResult> removeRecursoCommand,
                              IAsyncRequestHandler<UpdateRecursoCommand, OperationResult> updateRecursoCommand,
                              IConfiguration configuration)
    {
        _createRecursoCommand = createRecursoCommand;
        _listRecursoQuery = listRecursoQuery;
        _authQuery = authQuery;
        _getRecursoQuery = getRecursoQuery;
        _removeRecursoCommand = removeRecursoCommand;
        _updateRecursoCommand = updateRecursoCommand;
        _configuration = configuration;
    }

    [Authorize]
    public async UnaryResult<OperationResult> AddAsync(CreateRecursoCommand command)
    {
        return await _createRecursoCommand.InvokeAsync(command);
    }

    [Authorize]
    public async UnaryResult<IEnumerable<RecursoViewModel>> AllAsync(ListRecursoQuery query)
    {
        return await _listRecursoQuery.InvokeAsync(query);
    }

    public async UnaryResult<AuthResponse> AuthAsync(AuthQuery query)
    {
        AuthResponse response = await _authQuery.InvokeAsync(query);

        if (response.Status == OperationResult.Success)
        {
            int.TryParse(_configuration["Jwt:Expires"], out int jwtExpires);

            response.Recurso.Token = TokenService.GenerateToken(response.Recurso.Id.ToString(), _configuration["Jwt:Key"], _configuration["Jwt:Issuer"], jwtExpires);
        }

        return response;
    }

    [Authorize]
    public async UnaryResult<RecursoViewModel> GetAsync(GetRecursoQuery query)
    {
        return await _getRecursoQuery.InvokeAsync(query);
    }

    [Authorize]
    public async UnaryResult<OperationResult> RemoveAsync(RemoveRecursoCommand command)
    {
        return await _removeRecursoCommand.InvokeAsync(command);
    }

    [Authorize]
    public async UnaryResult<OperationResult> UpdateAsync(UpdateRecursoCommand command)
    {
        return await _updateRecursoCommand.InvokeAsync(command);
    }
}
