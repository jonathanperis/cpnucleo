using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Impedimento
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IRepository<ImpedimentoItem> _impedimentoRepository;

        public RemoverModel(IRepository<ImpedimentoItem> impedimentoRepository) => _impedimentoRepository = impedimentoRepository;

        [BindProperty]
        public ImpedimentoItem Impedimento { get; set; }

        public async Task<IActionResult> OnGetAsync(int idImpedimento)
        {
            Impedimento = await _impedimentoRepository.Consultar(idImpedimento);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(ImpedimentoItem impedimento)
        {
            if (!ModelState.IsValid) return Page();

            await _impedimentoRepository.Remover(impedimento);

            return RedirectToPage("Listar");
        }
    }
}