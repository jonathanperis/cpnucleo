namespace GrpcServer.Common.Security;

public static class UserAdministration
{
    public static void RequireAdmin(IHttpContextAccessor context)
    {
        var user = context.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true || !user.HasClaim(CpnucleoClaimTypes.Admin, "true"))
            throw new Grpc.Core.RpcException(new Grpc.Core.Status(Grpc.Core.StatusCode.PermissionDenied, "User administration requires an administrator."));
    }
}
