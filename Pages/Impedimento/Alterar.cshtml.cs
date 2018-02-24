using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.Impedimento;
using Microsoft.AspNetCore.Authorization;
using dotnet_cpnucleo_pages.Repository;

namespace dotnet_cpnucleo_pages.Pages.Impedimento
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IRepository<ImpedimentoItem> _ImpedimentoRepository;

        public AlterarModel(IRepository<ImpedimentoItem> ImpedimentoRepository)
        {
            _ImpedimentoRepository = ImpedimentoRepository;
        }

        [BindProperty]
        public ImpedimentoItem Impedimento { get; set; }

        public async Task<IActionResult> OnGetAsync(int idImpedimento)
        {
            Impedimento = await _ImpedimentoRepository.Consultar(idImpedimento);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(ImpedimentoItem impedimento)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _ImpedimentoRepository.Alterar(impedimento);

            return RedirectToPage("Listar");
        }
    }
}