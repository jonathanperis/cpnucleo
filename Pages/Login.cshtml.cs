using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnet_cpnucleo_pages.Authentication;
using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;
using dotnet_cpnucleo_pages.Repository.Recurso;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dotnet_cpnucleo_pages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IRecursoRepository _RecursoRepository;

        public LoginModel(IRecursoRepository RecursoRepository)
        {
            _RecursoRepository = RecursoRepository;
        }

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

            var RecursoItem = _RecursoRepository.ValidarRecurso(login.Usuario, login.Senha, out bool valido);

            if (!valido)
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                return Page();
            }

            ClaimsPrincipal principal = ClaimsManager.CreateClaimsPrincipal(ClaimTypes.PrimarySid, RecursoItem.IdRecurso.ToString());
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

    public class LoginItem
    {
        public int IdRecurso { get; set; }

        [Display(Name = "Login")]
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        public string Usuario { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Necessário informar a {0}.")]
        public string Senha { get; set; }
    }
}