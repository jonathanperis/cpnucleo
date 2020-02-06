using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly IImpedimentoTarefaApiService _impedimentoTarefaApiService;
        private readonly ICrudApiService<ImpedimentoViewModel> _impedimentoApiService;
        private readonly ITarefaApiService _tarefaApiService;

        public IncluirModel(IClaimsManager claimsManager,
                                    IImpedimentoTarefaApiService impedimentoTarefaApiService,
                                    ICrudApiService<ImpedimentoViewModel> impedimentoApiService,
                                    ITarefaApiService tarefaApiService)
            : base(claimsManager)
        {
            _impedimentoTarefaApiService = impedimentoTarefaApiService;
            _impedimentoApiService = impedimentoApiService;
            _tarefaApiService = tarefaApiService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }

        public IActionResult OnGet(Guid idTarefa)
        {
            Tarefa = _tarefaApiService.Consultar(Token, idTarefa);

            SelectImpedimentos = new SelectList(_impedimentoApiService.Listar(Token), "Id", "Nome");

            return Page();
        }

        public IActionResult OnPost(Guid idTarefa)
        {
            if (!ModelState.IsValid)
            {
                Tarefa = _tarefaApiService.Consultar(Token, idTarefa);

                SelectImpedimentos = new SelectList(_impedimentoApiService.Listar(Token), "Id", "Nome");

                return Page();
            }

            _impedimentoTarefaApiService.Incluir(Token, ImpedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa });
        }
    }
}