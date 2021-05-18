using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class AlterarModel : PageBase
    {
        private readonly ICpnucleoApiService _cpnucleoApiService;

        public AlterarModel(ICpnucleoApiService cpnucleoApiService)
        {
            _cpnucleoApiService = cpnucleoApiService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                ImpedimentoTarefa = await _cpnucleoApiService.GetAsync<ImpedimentoTarefaViewModel>("impedimentoTarefa", Token, id);

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
                    ImpedimentoTarefa = await _cpnucleoApiService.GetAsync<ImpedimentoTarefaViewModel>("impedimentoTarefa", Token, ImpedimentoTarefa.Id);

                    IEnumerable<ImpedimentoViewModel> result = await _cpnucleoApiService.GetAsync<IEnumerable<ImpedimentoViewModel>>("impedimento", Token);
                    SelectImpedimentos = new SelectList(result, "Id", "Nome");

                    return Page();
                }

                await _cpnucleoApiService.PutAsync("impedimentoTarefa", Token, ImpedimentoTarefa.Id, ImpedimentoTarefa);

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