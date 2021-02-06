using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Apontamento
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly IHttpService _httpService;

        public RemoverModel(IHttpService httpService)
        {
            _httpService = httpService;
        }

        [BindProperty]
        public ApontamentoViewModel Apontamento { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                var result = await _httpService.GetAsync<ApontamentoViewModel>("apontamento", Token, id);

                if (!result.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                    return Page();
                }

                Apontamento = result.response;

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
                    var result = await _httpService.GetAsync<ApontamentoViewModel>("apontamento", Token, Apontamento.Id);

                    if (!result.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                        return Page();
                    }

                    Apontamento = result.response;

                    return Page();               
                }

                var result2 = await _httpService.DeleteAsync<ApontamentoViewModel>("apontamento", Token, Apontamento.Id);

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