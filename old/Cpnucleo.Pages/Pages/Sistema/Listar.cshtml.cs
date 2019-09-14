using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Sistema
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IRepository<SistemaModel> _sistemaRepository;

        public ListarModel(IRepository<SistemaModel> sistemaRepository) => _sistemaRepository = sistemaRepository;

        public SistemaModel Sistema { get; set; }

        public IEnumerable<SistemaModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _sistemaRepository.ListarAsync();

            return Page();
        }
    }
}