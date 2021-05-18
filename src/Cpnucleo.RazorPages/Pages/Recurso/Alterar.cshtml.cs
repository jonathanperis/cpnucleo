using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Recurso
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
        public RecursoViewModel Recurso { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                Recurso = await _cpnucleoApiService.GetAsync<RecursoViewModel>("recurso", Token, id);

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
                    Recurso = await _cpnucleoApiService.GetAsync<RecursoViewModel>("recurso", Token, Recurso.Id);

                    return Page();
                }

                await _cpnucleoApiService.PutAsync("recurso", Token, Recurso.Id, Recurso);

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