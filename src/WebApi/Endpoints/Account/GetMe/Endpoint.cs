namespace WebApi.Endpoints.Account.GetMe;

// Dapper (self-service account store)
public class Endpoint(AccountStore accounts) : EndpointWithoutRequest<Response>
{
    public override void Configure()
    {
        Get("/me");
        Description(x => x.WithTags("Account"));

        Summary(s =>
        {
            s.Summary = "Retrieve the signed-in user's profile";
            s.Description = "Returns the account of the token's subject. Any signed-in user (or service account) may call it; a subject without an active account is 404.";
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var user = await accounts.FindCurrentAsync(cancellationToken);
        if (user is null)
        {
            Logger.LogWarning("The token's subject has no active account.");
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Response.Id = user.Id;
        Response.Name = user.Name;
        Response.Login = user.Login;
        Response.CreatedAt = user.CreatedAt;
        Response.UpdatedAt = user.UpdatedAt;

        await Send.OkAsync(Response, cancellationToken);
    }
}
