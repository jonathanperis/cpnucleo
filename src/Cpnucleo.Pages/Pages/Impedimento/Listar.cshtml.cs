using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Impedimento
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IRepository<ImpedimentoItem> _impedimentoRepository;

        public ListarModel(IRepository<ImpedimentoItem> impedimentoRepository) => _impedimentoRepository = impedimentoRepository;

        [BindProperty]
        public ImpedimentoItem Impedimento { get; set; }

        [BindProperty]
        public IEnumerable<ImpedimentoItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _impedimentoRepository.Listar();

            return Page();
        }
    }
}