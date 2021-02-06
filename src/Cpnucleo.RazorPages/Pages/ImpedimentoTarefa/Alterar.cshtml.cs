using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class AlterarModel : PageBase
    {
        private readonly IHttpService _httpService;

        public AlterarModel(IHttpService httpService)
        {
            _httpService = httpService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                var result = await _httpService.GetAsync<ImpedimentoTarefaViewModel>("impedimentoTarefa", Token, id);

                if (!result.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                    return Page();
                }

                ImpedimentoTarefa = result.response; 

                var result2 = await _httpService.GetAsync<IEnumerable<ImpedimentoViewModel>>("impedimento", Token);

                if (!result2.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                    return Page();
                }

                SelectImpedimentos = new SelectList(result2.response, "Id", "Nome");

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
                    var result = await _httpService.GetAsync<ImpedimentoTarefaViewModel>("impedimentoTarefa", Token, ImpedimentoTarefa.Id);

                    if (!result.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                        return Page();
                    }

                    ImpedimentoTarefa = result.response; 

                    var result2 = await _httpService.GetAsync<IEnumerable<ImpedimentoViewModel>>("impedimento", Token);

                    if (!result2.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                        return Page();
                    }

                    SelectImpedimentos = new SelectList(result2.response, "Id", "Nome");

                    return Page();
                }

                var result3 = await _httpService.PutAsync<ImpedimentoTarefaViewModel>("impedimentoTarefa", Token, ImpedimentoTarefa.Id, ImpedimentoTarefa);

                if (!result3.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result3.code} - {result3.message}");
                    return Page();
                }

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