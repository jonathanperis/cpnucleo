using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Projeto
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
        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectSistemas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                IEnumerable<SistemaViewModel> result = await _cpnucleoApiService.GetAsync<IEnumerable<SistemaViewModel>>("sistema", Token);
                SelectSistemas = new SelectList(result, "Id", "Nome");

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
                    IEnumerable<SistemaViewModel> result = await _cpnucleoApiService.GetAsync<IEnumerable<SistemaViewModel>>("sistema", Token);
                    SelectSistemas = new SelectList(result, "Id", "Nome");

                    return Page();
                }

                await _cpnucleoApiService.PostAsync<ProjetoViewModel>("projeto", Token, Projeto);

                return RedirectToPage("Listar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}