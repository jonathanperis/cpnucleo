using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IImpedimentoTarefaGrpcService _impedimentoTarefaGrpcService;
        private readonly ICrudGrpcService<ImpedimentoViewModel> _impedimentoGrpcService;
        private readonly ITarefaGrpcService _tarefaGrpcService;

        public IncluirModel(IImpedimentoTarefaGrpcService impedimentoTarefaGrpcService,
                                    ICrudGrpcService<ImpedimentoViewModel> impedimentoGrpcService,
                                    ITarefaGrpcService tarefaGrpcService)
        {
            _impedimentoTarefaGrpcService = impedimentoTarefaGrpcService;
            _impedimentoGrpcService = impedimentoGrpcService;
            _tarefaGrpcService = tarefaGrpcService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }

        public async Task<IActionResult> OnGet(Guid idTarefa)
        {
            Tarefa = await _tarefaGrpcService.ConsultarAsync(idTarefa);

            SelectImpedimentos = new SelectList(await _impedimentoGrpcService.ListarAsync(), "Id", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPost(Guid idTarefa)
        {
            if (!ModelState.IsValid)
            {
                Tarefa = await _tarefaGrpcService.ConsultarAsync(idTarefa);

                SelectImpedimentos = new SelectList(await _impedimentoGrpcService.ListarAsync(), "Id", "Nome");

                return Page();
            }

            await _impedimentoTarefaGrpcService.IncluirAsync(ImpedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa });
        }
    }
}