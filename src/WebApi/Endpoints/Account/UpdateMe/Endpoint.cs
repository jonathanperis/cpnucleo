namespace WebApi.Endpoints.Account.UpdateMe;

// Dapper (self-service account store)
public class Endpoint(AccountStore accounts) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/me");
        Description(x => x.WithTags("Account"));

        Summary(s =>
        {
            s.Summary = "Change the signed-in user's display name";
            s.Description = "Updates only the display name of the token's subject. The login, password and other accounts can't be changed here.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        var user = await accounts.FindCurrentAsync(cancellationToken);
        if (user is null)
        {
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        // The domain owns the name rules; a null login and password keep the current ones.
        Domain.Entities.User.Update(user, request.Name);
        Response.Success = await accounts.SaveNameAsync(user, cancellationToken);
        if (!Response.Success)
        {
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("User {UserId} changed their display name.", user.Id);
        HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>().NotifyChanged(nameof(Domain.Entities.User));

        await Send.OkAsync(Response, cancellationToken);
    }
}
