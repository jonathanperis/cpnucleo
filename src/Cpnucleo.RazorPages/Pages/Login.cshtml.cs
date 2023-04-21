using Cpnucleo.RazorPages.Services;
using Cpnucleo.Shared.Queries.AuthUser;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Cpnucleo.RazorPages.Pages;

public class LoginModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoAuthApiClient;
    private readonly IConfiguration _configuration;

    public LoginModel(ICpnucleoApiClient cpnucleoApiClient, IConfiguration configuration)
    {
        _cpnucleoAuthApiClient = cpnucleoApiClient;
        _configuration = configuration;
    }

    [BindProperty]
    public AuthDto Auth { get; set; }

    public async Task<IActionResult> OnGetAsync(string? returnUrl = null, bool logout = false)
    {
        try
        {
            if (logout)
            {
                await HttpContext.SignOutAsync();

                return RedirectToPage("Login");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _cpnucleoAuthApiClient.ExecuteAsync<AuthUserViewModel>("AuthUser", "AuthUser", new AuthUserQuery(Auth.Usuario, Auth.Senha));

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                return Page();
            }
            else
            {
                IEnumerable<Claim> claims = new[]
                {
                    new Claim(ClaimTypes.PrimarySid, result.Recurso.Id.ToString()),
                    new Claim(ClaimTypes.Hash, result.Token)
                };

                var principal = ClaimsService.CreateClaimsPrincipal(claims);

                int.TryParse(_configuration["Cookie:Expires"], out var expiresUtc);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(expiresUtc)
                    });

                return RedirectToLocal(returnUrl);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToPage("Apontamento/Listar");
        }
    }
}
