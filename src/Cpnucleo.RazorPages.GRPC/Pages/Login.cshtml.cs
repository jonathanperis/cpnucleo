using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.GRPC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Cpnucleo.RazorPages.GRPC.Pages
{
    public class LoginModel : PageBase
    {
        private readonly IClaimsManager _claimsManager;
        private readonly IRecursoApiService _recursoApiService;

        public LoginModel(IClaimsManager claimsManager,
                                    IRecursoApiService recursoApiService)
            : base(claimsManager)
        {
            _claimsManager = claimsManager;
            _recursoApiService = recursoApiService;
        }

        [BindProperty]
        public LoginViewModel Login { get; set; }

        public IActionResult OnGet(string returnUrl = null, bool logout = false)
        {
            if (logout)
            {
                HttpContext.SignOutAsync();

                return RedirectToPage("Login");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return Page();
        }

        public IActionResult OnPost(string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            RecursoViewModel recurso = _recursoApiService.Autenticar(Login.Usuario, Login.Senha);

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

            ClaimsPrincipal principal = _claimsManager.CreateClaimsPrincipal(claims);

            HttpContext.SignInAsync(principal);

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