using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.Impedimento;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using dotnet_cpnucleo_pages.Repository;

namespace dotnet_cpnucleo_pages.Pages.Impedimento
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IRepository<ImpedimentoItem> _impedimentoRepository;

        public ListarModel(IRepository<ImpedimentoItem> impedimentoRepository)
        {
            _impedimentoRepository = impedimentoRepository;
        }

        [BindProperty]
        public ImpedimentoItem Impedimento { get; set; }

        [BindProperty]
        public IList<ImpedimentoItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _impedimentoRepository.Listar();

            return Page();
        }
    }
}