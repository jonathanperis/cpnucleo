using Cpnucleo.Infra.CrossCutting.Util.Requests.Auth;

namespace Cpnucleo.GRPC.Services;

public class AuthGrpcService : ServiceBase<IAuthGrpcService>, IAuthGrpcService
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public AuthGrpcService(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    public async UnaryResult<AuthResponse> AuthAsync(AuthRequest query)
    {
        AuthResponse response = await _mediator.Send(query);

        if (response.Status == OperationResult.Success)
        {
            int.TryParse(_configuration["Jwt:Expires"], out int jwtExpires);

            response.Recurso.Token = TokenService.GenerateToken(response.Recurso.Id.ToString(), _configuration["Jwt:Key"], _configuration["Jwt:Issuer"], jwtExpires);
        }

        return response;
    }
}
