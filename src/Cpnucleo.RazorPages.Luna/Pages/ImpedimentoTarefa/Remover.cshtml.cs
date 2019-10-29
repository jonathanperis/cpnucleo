using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Luna.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IImpedimentoTarefaAppService _impedimentoTarefaAppService;

        public RemoverModel(IImpedimentoTarefaAppService impedimentoTarefaAppService)
        {
            _impedimentoTarefaAppService = impedimentoTarefaAppService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public IActionResult OnGet(Guid id)
        {
            ImpedimentoTarefa = _impedimentoTarefaAppService.Consultar(id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _impedimentoTarefaAppService.Remover(ImpedimentoTarefa.Id);

            return RedirectToPage("Listar", new { idTarefa = ImpedimentoTarefa.IdTarefa });
        }
    }
}