using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Apontamento
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IApontamentoRepository _apontamentoRepository;

        public RemoverModel(IApontamentoRepository apontamentoRepository) => _apontamentoRepository = apontamentoRepository;

        [BindProperty(SupportsGet = true)]
        public ApontamentoItem Apontamento { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Apontamento = await _apontamentoRepository.Consultar(Apontamento.IdApontamento);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _apontamentoRepository.Remover(Apontamento);

            return RedirectToPage("/Apontamento");
        }
    }
}