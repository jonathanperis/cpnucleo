using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.Sistema;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using dotnet_cpnucleo_pages.Repository;

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
        public IList<SistemaItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _sistemaRepository.Listar();

            return Page();
        }
    }
}