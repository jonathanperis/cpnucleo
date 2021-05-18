using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Sistema
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
        public SistemaViewModel Sistema { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                Sistema = await _cpnucleoApiService.GetAsync<SistemaViewModel>("sistema", Token, id);

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
                    Sistema = await _cpnucleoApiService.GetAsync<SistemaViewModel>("sistema", Token, Sistema.Id);

                    return Page();
                }

                await _cpnucleoApiService.PutAsync("sistema", Token, Sistema.Id, Sistema);

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