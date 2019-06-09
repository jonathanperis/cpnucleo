using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Projeto
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IRepository<ProjetoItem> _projetoRepository;

        public ListarModel(IRepository<ProjetoItem> projetoRepository) => _projetoRepository = projetoRepository;

        public ProjetoItem Projeto { get; set; }

        public IEnumerable<ProjetoItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _projetoRepository.Listar();

            return Page();
        }
    }
}