using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Impedimento
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
        public ImpedimentoViewModel Impedimento { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                var result = await _httpService.GetAsync<ImpedimentoViewModel>("impedimento", Token, id);

                if (!result.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                    return Page();
                }

                Impedimento = result.response; 

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
                    var result = await _httpService.GetAsync<ImpedimentoViewModel>("impedimento", Token, Impedimento.Id);

                    if (!result.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                        return Page();
                    }

                    Impedimento = result.response;  
                                        
                    return Page();
                }

                var result2 = await _httpService.PutAsync<ImpedimentoViewModel>("impedimento", Token, Impedimento.Id, Impedimento);

                if (!result2.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                    return Page();
                }

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