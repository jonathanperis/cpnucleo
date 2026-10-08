namespace GrpcServer.Common.Security;

/// <summary>The user administration policy: the same admin claim WebApi requires for its user endpoints.</summary>
public static class UserAdministration
{
    public const string Policy = "UserAdministration";
    public const string DeniedMessage = "User administration requires an administrator.";
}
