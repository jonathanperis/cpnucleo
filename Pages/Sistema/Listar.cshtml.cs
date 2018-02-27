using dotnet_cpnucleo_pages.Repository2;
using dotnet_cpnucleo_pages.Repository2.Sistema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Pages.Sistema
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IRepository<SistemaItem> _sistemaRepository;

        public ListarModel(IRepository<SistemaItem> sistemaRepository)
        {
            _sistemaRepository = sistemaRepository;
        }

        [BindProperty]
        public SistemaItem Sistema { get; set; }

        [BindProperty]
        public IEnumerable<SistemaItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _sistemaRepository.Listar();

            return Page();
        }
    }
}