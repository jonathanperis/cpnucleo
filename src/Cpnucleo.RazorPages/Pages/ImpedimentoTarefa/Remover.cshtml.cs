using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly IImpedimentoTarefaService _impedimentoTarefaService;

        public RemoverModel(IImpedimentoTarefaService impedimentoTarefaService)
        {
            _impedimentoTarefaService = impedimentoTarefaService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                ImpedimentoTarefa = await _impedimentoTarefaService.ConsultarAsync(Token, id);

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
                await _impedimentoTarefaService.RemoverAsync(Token, ImpedimentoTarefa.Id);

                return RedirectToPage("Listar", new { idTarefa = ImpedimentoTarefa.IdTarefa });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}