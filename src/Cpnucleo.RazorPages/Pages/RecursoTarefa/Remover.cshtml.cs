using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.RecursoTarefa
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly IRecursoTarefaService _recursoTarefaService;

        public RemoverModel(IRecursoTarefaService recursoTarefaService)
        {
            _recursoTarefaService = recursoTarefaService;
        }

        [BindProperty]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                RecursoTarefa = await _recursoTarefaService.ConsultarAsync(Token, id);

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _recursoTarefaService.RemoverAsync(Token, RecursoTarefa.Id);

                return RedirectToPage("Listar", new { idTarefa = RecursoTarefa.IdTarefa });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}