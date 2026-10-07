using Open.IdentityServer.Services;

namespace IdentityApi.Endpoints.Account.Error;

public class Request
{
    /// <summary>The error id the server appended when it redirected to the sign-in page.</summary>
    public string? ErrorId { get; set; }
}

public class Response
{
    /// <summary>The OAuth/OpenID Connect error code, for example <c>invalid_request</c>.</summary>
    public string? Error { get; set; }

    /// <summary>A short explanation safe to show users.</summary>
    public string? ErrorDescription { get; set; }
}

/// <summary>
/// Lets the WebClient sign-in page explain a rejected authorization request (for example an
/// unregistered redirect URI). Only the error code and description are returned.
/// </summary>
public class Endpoint(IIdentityServerInteractionService interaction) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/account/error");
        AllowAnonymous();
        Description(x => x.WithTags("Account"));
        Summary(s => s.Summary = "Describe a rejected authorization request");
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken)
    {
        var message = string.IsNullOrWhiteSpace(req.ErrorId) ? null : await interaction.GetErrorContextAsync(req.ErrorId);
        if (message is null)
        {
            await Send.NotFoundEnvelopeAsync("The sign-in error is unknown or has expired.", cancellationToken);
            return;
        }

        Response.Error = message.Error;
        Response.ErrorDescription = message.ErrorDescription;
        await Send.OkAsync(Response, cancellationToken);
    }
}
