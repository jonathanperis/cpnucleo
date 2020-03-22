using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

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

        public async Task<IActionResult> OnGet(Guid idTarefa)
        {
            Tarefa = await _tarefaApiService.ConsultarAsync(Token, idTarefa);

            SelectImpedimentos = new SelectList(await _impedimentoApiService.ListarAsync(Token), "Id", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPost(Guid idTarefa)
        {
            if (!ModelState.IsValid)
            {
                Tarefa = await _tarefaApiService.ConsultarAsync(Token, idTarefa);

                SelectImpedimentos = new SelectList(await _impedimentoApiService.ListarAsync(Token), "Id", "Nome");

                return Page();
            }

            await _impedimentoTarefaApiService.IncluirAsync(Token, ImpedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa });
        }
    }
}