using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Pages.Recurso
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IRecursoAppService _recursoAppService;

        public AlterarModel(IRecursoAppService recursoAppService)
        {
            _recursoAppService = recursoAppService;
        }

        [BindProperty]
        public RecursoViewModel Recurso { get; set; }

        public IActionResult OnGet(Guid idRecurso)
        {
            Recurso = _recursoAppService.Consultar(idRecurso);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _recursoAppService.Alterar(Recurso);

            return RedirectToPage("Listar");
        }
    }
}