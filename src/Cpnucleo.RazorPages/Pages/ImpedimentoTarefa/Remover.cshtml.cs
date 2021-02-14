using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly ICpnucleoApiService _cpnucleoApiService;

        public RemoverModel(ICpnucleoApiService cpnucleoApiService)
        {
            _cpnucleoApiService = cpnucleoApiService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                ImpedimentoTarefa = await _cpnucleoApiService.GetAsync<ImpedimentoTarefaViewModel>("impedimentoTarefa", Token, id);

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

                    return Page();
                }

                await _cpnucleoApiService.DeleteAsync("impedimentoTarefa", Token, ImpedimentoTarefa.Id);

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