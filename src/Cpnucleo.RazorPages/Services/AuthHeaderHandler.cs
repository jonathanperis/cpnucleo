using System.Net.Http.Headers;
using System.Security.Claims;

namespace Cpnucleo.RazorPages.Services;

 internal class AuthHeaderHandler : DelegatingHandler
 {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthHeaderHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Token => ClaimsService.ReadClaimsPrincipal(_httpContextAccessor.HttpContext.User, ClaimTypes.Hash);

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //potentially refresh token here if it has expired etc.

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}