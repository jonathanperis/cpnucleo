using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly IImpedimentoTarefaService _impedimentoTarefaService;

        public ListarModel(IImpedimentoTarefaService impedimentoTarefaService)
        {
            _impedimentoTarefaService = impedimentoTarefaService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public IEnumerable<ImpedimentoTarefaViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid idTarefa)
        {
            try
            {
                Lista = await _impedimentoTarefaService.ListarPorTarefaAsync(Token, idTarefa);

                ViewData["idTarefa"] = idTarefa;

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}