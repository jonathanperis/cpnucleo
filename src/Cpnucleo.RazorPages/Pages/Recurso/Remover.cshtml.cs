using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Pages.Recurso
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IRecursoAppService _recursoAppService;

        public RemoverModel(IRecursoAppService recursoAppService) => _recursoAppService = recursoAppService;

        [BindProperty]
        public RecursoViewModel Recurso { get; set; }

        public IActionResult OnGet(Guid idRecurso)
        {
            Recurso = _recursoAppService.Consultar(idRecurso);

            return Page();
        }

        public IActionResult OnPost()
        {
            _recursoAppService.Remover(Recurso.Id);

            return RedirectToPage("Listar");
        }
    }
}