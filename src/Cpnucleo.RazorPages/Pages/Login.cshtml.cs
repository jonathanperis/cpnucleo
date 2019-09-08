using Cpnucleo.Application.Interfaces;
using Cpnucleo.RazorPages.Authentication;
using Cpnucleo.RazorPages.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Cpnucleo.RazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IClaimsManager _claimsManager;

        private readonly IRecursoAppService _recursoAppService;

        public LoginModel(IClaimsManager claimsManager, IRecursoAppService recursoAppService)
        {
            _claimsManager = claimsManager;

            _recursoAppService = recursoAppService;
        }

        [BindProperty]
        public LoginViewModel Login { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
        }

        public IActionResult OnGetLogout()
        {
            HttpContext.SignOutAsync();

            return RedirectToPage("Login");
        }

        public IActionResult OnPost(string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var recurso = _recursoAppService.ValidarRecurso(Login.Usuario, Login.Senha, out bool valido);

            if (!valido)
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                return Page();
            }

            ClaimsPrincipal principal = _claimsManager.CreateClaimsPrincipal(ClaimTypes.PrimarySid, recurso.Id.ToString());

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
                return RedirectToPage("Apontamento");
            }
        }
    }
}