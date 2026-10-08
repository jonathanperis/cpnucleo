using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;

namespace GrpcServer.Common.Security;

/// <summary>
/// Handlers declare their policy with <c>[Authorize]</c> (the handler server copies it into the
/// endpoint metadata). A forbidden gRPC call is answered as a gRPC PermissionDenied status with a
/// client-safe message instead of a bare HTTP 403, so clients see why it was refused.
/// </summary>
internal sealed class GrpcAuthorizationResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler fallback = new();

    public Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        if (!authorizeResult.Forbidden || context.Request.ContentType?.StartsWith("application/grpc", StringComparison.OrdinalIgnoreCase) != true)
            return fallback.HandleAsync(next, context, policy, authorizeResult);

        var requiresAdmin = policy.Requirements.OfType<ClaimsAuthorizationRequirement>().Any(requirement => requirement.ClaimType == CpnucleoClaimTypes.Admin);
        // A "trailers-only" gRPC response: HTTP 200 with the status in the headers.
        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "application/grpc";
        context.Response.Headers["grpc-status"] = ((int)Grpc.Core.StatusCode.PermissionDenied).ToString(System.Globalization.CultureInfo.InvariantCulture);
        context.Response.Headers["grpc-message"] = requiresAdmin ? UserAdministration.DeniedMessage : "You do not have permission to perform this action.";
        return Task.CompletedTask;
    }
}
