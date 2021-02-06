using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.RecursoProjeto
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly IHttpService _httpService;

        public ListarModel(IHttpService httpService)
        {
            _httpService = httpService;
        }

        [BindProperty]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }

        public IEnumerable<RecursoProjetoViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid idProjeto)
        {
            try
            {
                var result = await _httpService.GetAsync<IEnumerable<RecursoProjetoViewModel>>("recursoProjeto/getByProjeto", Token, idProjeto);

                if (!result.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                    return Page();
                }

                Lista = result.response;

                ViewData["idProjeto"] = idProjeto;

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