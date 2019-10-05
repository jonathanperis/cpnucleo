using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Pages.Impedimento
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly ICrudAppService<ImpedimentoViewModel> _impedimentoAppService;

        public RemoverModel(ICrudAppService<ImpedimentoViewModel> impedimentoAppService)
        {
            _impedimentoAppService = impedimentoAppService;
        }

        [BindProperty]
        public ImpedimentoViewModel Impedimento { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Impedimento = _impedimentoAppService.Consultar(id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _impedimentoAppService.Remover(Impedimento.Id);

            return RedirectToPage("Listar");
        }
    }
}