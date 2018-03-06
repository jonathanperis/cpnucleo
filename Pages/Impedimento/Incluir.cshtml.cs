using dotnet_cpnucleo_pages.Repository;
using dotnet_cpnucleo_pages.Repository.Impedimento;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Pages.Impedimento
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IRepository<ImpedimentoItem> _impedimentoRepository;

        public IncluirModel(IRepository<ImpedimentoItem> impedimentoRepository) => _impedimentoRepository = impedimentoRepository;

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

            await _impedimentoRepository.Incluir(impedimento);

            return RedirectToPage("Listar");
        }
    }
}