using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Recurso
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly IHttpService _httpService;

        public IncluirModel(IHttpService httpService)
        {
            _httpService = httpService;
        }

        [BindProperty]
        public RecursoViewModel Recurso { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var result = await _httpService.PostAsync<RecursoViewModel>("recurso", Token, Recurso);

                if (!result.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
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