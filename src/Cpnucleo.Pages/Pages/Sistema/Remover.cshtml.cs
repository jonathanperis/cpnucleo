using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Sistema
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IRepository<SistemaModel> _sistemaRepository;

        public RemoverModel(IRepository<SistemaModel> sistemaRepository) => _sistemaRepository = sistemaRepository;

        [BindProperty]
        public SistemaModel Sistema { get; set; }

        public async Task<IActionResult> OnGetAsync(int idSistema)
        {
            Sistema = await _sistemaRepository.ConsultarAsync(idSistema);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _sistemaRepository.RemoverAsync(Sistema);

            return RedirectToPage("Listar");
        }
    }
}