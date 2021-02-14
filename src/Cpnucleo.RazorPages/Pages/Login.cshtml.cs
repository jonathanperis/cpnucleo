using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Cpnucleo.RazorPages.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Cpnucleo.RazorPages.Pages
{
    public class LoginModel : PageBase
    {
        private readonly ICpnucleoApiService _cpnucleoApiService;
        private readonly IConfiguration _configuration;

        public LoginModel(ICpnucleoApiService cpnucleoApiService, IConfiguration configuration)
        {
            _cpnucleoApiService = cpnucleoApiService;
            _configuration = configuration;        
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

                RecursoViewModel recurso = await _cpnucleoApiService.PostAsync<RecursoViewModel>("recurso/auth", "", new RecursoViewModel { Login = Login.Usuario, Senha = Login.Senha });

                IEnumerable<Claim> claims = new[]
                {
                    new Claim(ClaimTypes.PrimarySid, recurso.Id.ToString()),
                    new Claim(ClaimTypes.Hash, recurso.Token)
                };

                ClaimsPrincipal principal = ClaimsService.CreateClaimsPrincipal(claims);

                int expiresUtc;
                int.TryParse(_configuration["Cookie:Expires"], out expiresUtc);

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