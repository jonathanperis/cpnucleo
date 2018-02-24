using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.Impedimento;
using Microsoft.AspNetCore.Authorization;
using dotnet_cpnucleo_pages.Repository;

namespace dotnet_cpnucleo_pages.Pages.Impedimento
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IRepository<ImpedimentoItem> _ImpedimentoRepository;

        public IncluirModel(IRepository<ImpedimentoItem> ImpedimentoRepository)
        {
            _ImpedimentoRepository = ImpedimentoRepository;
        }

        [BindProperty]
        public ImpedimentoItem Impedimento { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(ImpedimentoItem impedimento)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _ImpedimentoRepository.Incluir(impedimento);

            return RedirectToPage("Listar");
        }
    }
}