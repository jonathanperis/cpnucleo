namespace Cpnucleo.GRPC.Services;

public class AuthUserGrpcService : ServiceBase<IAuthUserGrpcService>, IAuthUserGrpcService
{
    private readonly IMediator _mediator;

    public AuthUserGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Autenticar Usuário
    /// </summary>
    /// <remarks>
    /// # Autenticar Usuário
    /// 
    /// Autentica o usuário no sistema.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<AuthUserViewModel> AuthUser(AuthUserQuery query)
    {
        return await _mediator.Send(query);
    }
}
