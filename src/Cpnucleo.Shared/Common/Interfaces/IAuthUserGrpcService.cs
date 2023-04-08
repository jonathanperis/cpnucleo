using Cpnucleo.Shared.Queries.AuthUser;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface IAuthUserGrpcService : IService<IAuthUserGrpcService>
{
    UnaryResult<AuthUserViewModel> AuthUser(AuthUserQuery query);
}