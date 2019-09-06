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
        private readonly IRepository<ImpedimentoModel> _impedimentoRepository;

        public RemoverModel(IRepository<ImpedimentoModel> impedimentoRepository) => _impedimentoRepository = impedimentoRepository;

        [BindProperty]
        public ImpedimentoModel Impedimento { get; set; }

        public async Task<IActionResult> OnGetAsync(int idImpedimento)
        {
            Impedimento = await _impedimentoRepository.ConsultarAsync(idImpedimento);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _impedimentoRepository.RemoverAsync(Impedimento);

            return RedirectToPage("Listar");
        }
    }
}