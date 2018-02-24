using System.Threading.Tasks;
using dotnet_cpnucleo_pages.Repository.Projeto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using dotnet_cpnucleo_pages.Repository;

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
        public IList<ProjetoItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _projetoRepository.Listar();

            return Page();
        }
    }
}