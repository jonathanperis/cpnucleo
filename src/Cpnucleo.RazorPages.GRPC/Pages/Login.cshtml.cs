using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.GRPC.Models;
using Cpnucleo.RazorPages.GRPC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IRecursoGrpcService _recursoGrpcService;
        private readonly ISystemConfiguration _systemConfiguration;

        public LoginModel(IRecursoGrpcService recursoGrpcService, ISystemConfiguration systemConfiguration)
        {
            _recursoGrpcService = recursoGrpcService;
            _systemConfiguration = systemConfiguration;
        }

        [BindProperty]
        public LoginViewModel Login { get; set; }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null, bool logout = false)
        {
            if (logout)
            {
                await HttpContext.SignOutAsync();

                return RedirectToPage("Login");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            RecursoViewModel recurso = await _recursoGrpcService.AutenticarAsync(Login.Usuario, Login.Senha);

            if (recurso.Id == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                return Page();
            }

            IEnumerable<Claim> claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid, recurso.Id.ToString()),
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