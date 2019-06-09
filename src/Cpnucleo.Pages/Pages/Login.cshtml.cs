using Cpnucleo.Pages.Authentication;
using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IRecursoRepository _recursoRepository;

        public LoginModel(IRecursoRepository recursoRepository) => _recursoRepository = recursoRepository;

        [BindProperty]
        public LoginItem Login { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
        }

        public async Task<IActionResult> OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync();

            return RedirectToPage("Login");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var recurso = _recursoRepository.ValidarRecurso(Login.Usuario, Login.Senha, out bool valido);

            if (!valido)
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                return Page();
            }

            ClaimsPrincipal principal = ClaimsManager.CreateClaimsPrincipal(ClaimTypes.PrimarySid, recurso.IdRecurso.ToString());
            await HttpContext.SignInAsync(principal);

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