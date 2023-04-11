namespace Cpnucleo.GRPC.Services;

public class AuthUserGrpcService : ServiceBase<IAuthUserGrpcService>, IAuthUserGrpcService
{
    private readonly ISender _sender;

    public AuthUserGrpcService(ISender sender)
    {
        _sender = sender;
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
        return await _sender.Send(query);
    }
}
