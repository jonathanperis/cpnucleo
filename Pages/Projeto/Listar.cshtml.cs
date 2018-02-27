using dotnet_cpnucleo_pages.Repository2;
using dotnet_cpnucleo_pages.Repository2.Projeto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Pages.Projeto
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IRepository<ProjetoItem> _projetoRepository;

        public ListarModel(IRepository<ProjetoItem> projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        [BindProperty]
        public ProjetoItem Projeto { get; set; }

        [BindProperty]
        public IEnumerable<ProjetoItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _projetoRepository.Listar();

            return Page();
        }
    }
}