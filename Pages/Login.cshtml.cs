using dotnet_cpnucleo_pages.Authentication;
using dotnet_cpnucleo_pages.Repository.Login;
using dotnet_cpnucleo_pages.Repository.Recurso;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Pages
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

        public async Task<IActionResult> OnPostAsync(LoginItem login, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var recursoItem = _recursoRepository.ValidarRecurso(login.Usuario, login.Senha, out bool valido);

            if (!valido)
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                return Page();
            }

            ClaimsPrincipal principal = ClaimsManager.CreateClaimsPrincipal(ClaimTypes.PrimarySid, recursoItem.IdRecurso.ToString());
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