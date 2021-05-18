using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.RecursoTarefa
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly IRecursoTarefaService _recursoTarefaService;

        public ListarModel(IRecursoTarefaService recursoTarefaService)
        {
            _recursoTarefaService = recursoTarefaService;
        }

        [BindProperty]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }

        public IEnumerable<RecursoTarefaViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid idTarefa)
        {
            try
            {
                var result = await _recursoTarefaService.ListarPorTarefaAsync(Token, idTarefa);

                if (!result.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                    return Page();
                }

                Lista = result.response;

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