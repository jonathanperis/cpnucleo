using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly ICpnucleoApiService _cpnucleoApiService;

        public IncluirModel(ICpnucleoApiService cpnucleoApiService)
        {
            _cpnucleoApiService = cpnucleoApiService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid idTarefa)
        {
            try
            {
                Tarefa = await _cpnucleoApiService.GetAsync<TarefaViewModel>("tarefa", Token, idTarefa);

                IEnumerable<ImpedimentoViewModel> result = await _cpnucleoApiService.GetAsync<IEnumerable<ImpedimentoViewModel>>("impedimento", Token);
                SelectImpedimentos = new SelectList(result, "Id", "Nome");

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
                if (!ModelState.IsValid)
                {
                    Tarefa = await _cpnucleoApiService.GetAsync<TarefaViewModel>("tarefa", Token, ImpedimentoTarefa.IdTarefa);

                    IEnumerable<ImpedimentoViewModel> result = await _cpnucleoApiService.GetAsync<IEnumerable<ImpedimentoViewModel>>("impedimento", Token);
                    SelectImpedimentos = new SelectList(result, "Id", "Nome");

                    return Page();
                }

                await _cpnucleoApiService.PostAsync<ImpedimentoTarefaViewModel>("impedimentoTarefa", Token, ImpedimentoTarefa);

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