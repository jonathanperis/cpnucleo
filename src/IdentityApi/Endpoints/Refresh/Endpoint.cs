namespace IdentityApi.Endpoints.Refresh;

public class Endpoint : EndpointWithoutRequest<Response>
{
    public override void Configure()
    {
        Post("/refresh");
        Description(x => x.WithTags("Authentication"));

        Summary(s =>
        {
            s.Summary = "Refresh authenticated user session";
            s.Description = "Issues a new 30-minute JWT for an already authenticated session.";
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Refreshing JWT token for an active session.");

        Response.Token = JwtBearer.CreateToken(o => { });

        await Send.OkAsync(Response, cancellationToken);
    }
}
