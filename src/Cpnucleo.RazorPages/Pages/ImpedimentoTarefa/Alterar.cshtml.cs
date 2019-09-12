using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IImpedimentoTarefaAppService _impedimentoTarefaAppService;

        private readonly IAppService<ImpedimentoViewModel> _impedimentoAppService;

        public AlterarModel(IImpedimentoTarefaAppService impedimentoTarefaAppService,
                                           IAppService<ImpedimentoViewModel> impedimentoAppService)
        {
            _impedimentoTarefaAppService = impedimentoTarefaAppService;
            _impedimentoAppService = impedimentoAppService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }

        public IActionResult OnGet(Guid idImpedimentoTarefa)
        {
            ImpedimentoTarefa = _impedimentoTarefaAppService.Consultar(idImpedimentoTarefa);
            SelectImpedimentos = new SelectList(_impedimentoAppService.Listar(), "Id", "Nome");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                SelectImpedimentos = new SelectList(_impedimentoAppService.Listar(), "Id", "Nome");

                return Page();
            }

            _impedimentoTarefaAppService.Alterar(ImpedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa = ImpedimentoTarefa.IdTarefa });
        }
    }
}