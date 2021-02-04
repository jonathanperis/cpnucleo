using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Cpnucleo.RazorPages.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages
{
    public class LoginModel : PageBase
    {
        private readonly IRecursoService _recursoService;
        private readonly ISystemConfiguration _systemConfiguration;

        public LoginModel(IRecursoService recursoService, ISystemConfiguration systemConfiguration)
        {
            _recursoService = recursoService;
            _systemConfiguration = systemConfiguration;
        }

        [BindProperty]
        public LoginViewModel Login { get; set; }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null, bool logout = false)
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

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                RecursoViewModel recurso = await _recursoService.AutenticarAsync(Login.Usuario, Login.Senha);

                if (recurso.Id == Guid.Empty)
                {
                    ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                    return Page();
                }

                IEnumerable<Claim> claims = new[]
                {
                    new Claim(ClaimTypes.PrimarySid, recurso.Id.ToString()),
                    new Claim(ClaimTypes.Hash, recurso.Token)
                };

                ClaimsPrincipal principal = ClaimsService.CreateClaimsPrincipal(claims);

                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(_systemConfiguration.CookieExpires)
                });

                return RedirectToLocal(returnUrl);
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
}