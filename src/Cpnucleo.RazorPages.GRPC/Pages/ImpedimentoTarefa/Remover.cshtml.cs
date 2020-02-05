using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IImpedimentoTarefaGrpcService _impedimentoTarefaGrpcService;

        public RemoverModel(IImpedimentoTarefaGrpcService impedimentoTarefaGrpcService)    
        {
            _impedimentoTarefaGrpcService = impedimentoTarefaGrpcService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            ImpedimentoTarefa = await _impedimentoTarefaGrpcService.ConsultarAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            await _impedimentoTarefaGrpcService.RemoverAsync(ImpedimentoTarefa.Id);

            return RedirectToPage("Listar", new { idTarefa = ImpedimentoTarefa.IdTarefa });
        }
    }
}