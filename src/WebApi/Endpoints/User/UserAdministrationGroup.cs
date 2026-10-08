namespace WebApi.Endpoints.User;

/// <summary>
/// User administration endpoints share their policy and tag here. The empty route prefix keeps
/// their existing <c>/user</c> and <c>/users</c> routes.
/// </summary>
public sealed class UserAdministrationGroup : Group
{
    public UserAdministrationGroup() => Configure("", endpoint =>
    {
        endpoint.Policies("UserAdministration");
        endpoint.Description(x => x.WithTags("Users"));
    });
}
