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
        private readonly IRepository<ImpedimentoModel> _impedimentoRepository;

        public ListarModel(IRepository<ImpedimentoModel> impedimentoRepository) => _impedimentoRepository = impedimentoRepository;

        public ImpedimentoModel Impedimento { get; set; }

        public IEnumerable<ImpedimentoModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _impedimentoRepository.ListarAsync();

            return Page();
        }
    }
}