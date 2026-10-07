namespace WebApi.Endpoints.Account.ChangePassword;

/// <summary>
/// Changes the signed-in user's own password after verifying the current one (Argon2id). Wrong
/// current passwords lock password changes for the account (<see cref="PasswordChangeThrottle"/>),
/// and verifications are concurrency-capped. The security stamp derives from the password hash, so
/// every token and sign-in session issued before the change stops working, exactly as when an
/// administrator changes the password. Passwords and login names are never logged.
/// </summary>
public class Endpoint(AccountStore accounts, IPasswordHasher passwordHasher, PasswordChangeThrottle throttle) : Endpoint<Request, Response>
{
    public const string ConcurrencyPolicy = "password-change-concurrency";

    public override void Configure()
    {
        Post("/me/password");
        Description(x => x.WithTags("Account"));
        // Each attempt costs one or two Argon2id hashes (64 MiB); cap how many run at once.
        Options(x => x.RequireRateLimiting(ConcurrencyPolicy));

        Summary(s =>
        {
            s.Summary = "Change the signed-in user's password";
            s.Description = "Verifies the current password and stores the new one. Existing tokens and sessions of the account stop working, so the client signs in again. Five wrong current passwords in 15 minutes lock password changes for 15 minutes (429 with Retry-After).";
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

        if (throttle.RetryAfter(user.Id) is { } retryAfter)
        {
            Logger.LogWarning("Password change rejected for user {UserId}: too many wrong current passwords.", user.Id);
            var seconds = Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds));
            HttpContext.Response.Headers.RetryAfter = seconds.ToString(System.Globalization.CultureInfo.InvariantCulture);
            await ApiErrors.WriteAsync(HttpContext, StatusCodes.Status429TooManyRequests,
                $"Too many incorrect passwords. Try again in {seconds} seconds.", cancellationToken: cancellationToken);
            return;
        }

        var verifiedHash = user.Password;
        if (verifiedHash is null || !passwordHasher.Verify(request.CurrentPassword, verifiedHash))
        {
            throttle.RecordFailure(user.Id);
            Logger.LogWarning("Password change rejected for user {UserId}: wrong current password.", user.Id);
            AddError(r => r.CurrentPassword, "The current password is incorrect.");
            ThrowIfAnyErrors();
        }

        throttle.RecordSuccess(user.Id);

        // The validator applied the same PasswordPolicy as user administration; a null login keeps the current login.
        Domain.Entities.User.Update(user, user.Name, passwordHash: passwordHasher.Hash(request.NewPassword));
        Response.Success = await accounts.SavePasswordAsync(user, verifiedHash!, cancellationToken);
        if (!Response.Success)
        {
            await ApiErrors.WriteAsync(HttpContext, StatusCodes.Status409Conflict,
                "The account changed concurrently. Sign in again and retry.", cancellationToken: cancellationToken);
            return;
        }

        Logger.LogInformation("User {UserId} changed their password.", user.Id);
        HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>().NotifyChanged(nameof(Domain.Entities.User));

        await Send.OkAsync(Response, cancellationToken);
    }
}
