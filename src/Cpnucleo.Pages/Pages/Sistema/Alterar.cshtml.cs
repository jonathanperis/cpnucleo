using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Sistema
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IRepository<SistemaItem> _sistemaRepository;

        public AlterarModel(IRepository<SistemaItem> sistemaRepository) => _sistemaRepository = sistemaRepository;

        [BindProperty]
        public SistemaItem Sistema { get; set; }

        public async Task<IActionResult> OnGetAsync(int idSistema)
        {
            Sistema = await _sistemaRepository.ConsultarAsync(idSistema);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _sistemaRepository.AlterarAsync(Sistema);

            return RedirectToPage("Listar");
        }
    }
}