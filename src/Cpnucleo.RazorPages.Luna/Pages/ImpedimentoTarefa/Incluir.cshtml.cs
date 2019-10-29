using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Cpnucleo.RazorPages.Luna.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IImpedimentoTarefaAppService _impedimentoTarefaAppService;
        private readonly IImpedimentoAppService _impedimentoAppService;
        private readonly ITarefaAppService _tarefaAppService;

        public IncluirModel(IImpedimentoTarefaAppService impedimentoTarefaAppService,
                            IImpedimentoAppService impedimentoAppService,
                            ITarefaAppService tarefaAppService)
        {
            _impedimentoTarefaAppService = impedimentoTarefaAppService;
            _impedimentoAppService = impedimentoAppService;
            _tarefaAppService = tarefaAppService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }

        public IActionResult OnGet(Guid idTarefa)
        {
            Tarefa = _tarefaAppService.Consultar(idTarefa);

            SelectImpedimentos = new SelectList(_impedimentoAppService.Listar(), "Id", "Nome");

            return Page();
        }

        public IActionResult OnPost(Guid idTarefa)
        {
            if (!ModelState.IsValid)
            {
                Tarefa = _tarefaAppService.Consultar(idTarefa);

                SelectImpedimentos = new SelectList(_impedimentoAppService.Listar(), "Id", "Nome");

                return Page();
            }

            _impedimentoTarefaAppService.Incluir(ImpedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa });
        }
    }
}