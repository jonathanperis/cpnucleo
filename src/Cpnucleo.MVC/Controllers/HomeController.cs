using Cpnucleo.Infra.CrossCutting.Shared.Requests.Auth;
using Cpnucleo.MVC.Services;
using Cpnucleo.MVC.Services.Interfaces;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Requests.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Cpnucleo.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ICpnucleoAuthApiClient _cpnucleoAuthApiClient;
    private readonly IConfiguration _configuration;

    private HomeViewModel _viewModel;

    public HomeController(ICpnucleoAuthApiClient cpnucleoApiService, IConfiguration configuration)
    {
        _cpnucleoAuthApiClient = cpnucleoApiService;
        _configuration = configuration;
    }

    public HomeViewModel ViewModel
    {
        get
        {
            if (_viewModel == null)
            {
                _viewModel = new HomeViewModel();
            }

            return _viewModel;
        }
        set => _viewModel = value;
    }

    [HttpGet]
    public async Task<IActionResult> Login(string? returnUrl = null, bool logout = false)
    {
        try
        {
            if (logout)
            {
                await HttpContext.SignOutAsync();

                return RedirectToAction("Login");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Login(HomeViewModel obj, string? returnUrl = null)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AuthResponse result = await _cpnucleoAuthApiClient.PostAsync<AuthResponse>("auth", new AuthRequest { Usuario = obj.Auth.Usuario, Senha = obj.Auth.Senha });

            if (result.Status == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                return View();
            }
            else
            {
                IEnumerable<Claim> claims = new[]
                {
                    new Claim(ClaimTypes.PrimarySid, result.Recurso.Id.ToString()),
                    new Claim(ClaimTypes.Hash, result.Token)
                };

                ClaimsPrincipal principal = ClaimsService.CreateClaimsPrincipal(claims);

                int.TryParse(_configuration["Cookie:Expires"], out int expiresUtc);

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
            return View();
        }
    }

    public IActionResult Erro()
    {
        return View();
    }

    public IActionResult Negado()
    {
        return View();
    }

    public IActionResult NaoEncontrado()
    {
        return View();
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction("Listar", "Apontamento");
        }
    }
}
