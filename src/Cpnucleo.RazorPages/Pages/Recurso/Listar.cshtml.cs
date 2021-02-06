using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Recurso
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly IHttpService _httpService;

        public ListarModel(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public RecursoViewModel Recurso { get; set; }

        public IEnumerable<RecursoViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var result = await _httpService.GetAsync<IEnumerable<RecursoViewModel>>("recurso", Token);

                if (!result.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                    return Page();
                }

                Lista = result.response;

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}