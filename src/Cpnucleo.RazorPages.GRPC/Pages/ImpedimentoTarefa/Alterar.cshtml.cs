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
    public class AlterarModel : PageModel
    {
        private readonly IImpedimentoTarefaGrpcService _impedimentoTarefaGrpcService;
        private readonly ICrudGrpcService<ImpedimentoViewModel> _impedimentoGrpcService;

        public AlterarModel(IImpedimentoTarefaGrpcService impedimentoTarefaGrpcService,
                                    ICrudGrpcService<ImpedimentoViewModel> impedimentoGrpcService)
        {
            _impedimentoTarefaGrpcService = impedimentoTarefaGrpcService;
            _impedimentoGrpcService = impedimentoGrpcService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            ImpedimentoTarefa = await _impedimentoTarefaGrpcService.ConsultarAsync(id);
            SelectImpedimentos = new SelectList(await _impedimentoGrpcService.ListarAsync(), "Id", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                SelectImpedimentos = new SelectList(await _impedimentoGrpcService.ListarAsync(), "Id", "Nome");

                return Page();
            }

            await _impedimentoTarefaGrpcService.AlterarAsync(ImpedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa = ImpedimentoTarefa.IdTarefa });
        }
    }
}