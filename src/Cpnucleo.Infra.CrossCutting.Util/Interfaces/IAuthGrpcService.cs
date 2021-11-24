using Cpnucleo.Infra.CrossCutting.Util.Requests.Auth;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

public interface IAuthGrpcService : IService<IAuthGrpcService>
{
    UnaryResult<AuthResponse> AuthAsync(AuthRequest query);
}