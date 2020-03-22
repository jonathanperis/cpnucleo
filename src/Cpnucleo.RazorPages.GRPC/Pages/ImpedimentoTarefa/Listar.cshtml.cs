using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IImpedimentoTarefaGrpcService _impedimentoTarefaGrpcService;

        public ListarModel(IImpedimentoTarefaGrpcService impedimentoTarefaGrpcService)
        {
            _impedimentoTarefaGrpcService = impedimentoTarefaGrpcService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public IEnumerable<ImpedimentoTarefaViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid idTarefa)
        {
            Lista = await _impedimentoTarefaGrpcService.ListarPorTarefaAsync(idTarefa);

            ViewData["idTarefa"] = idTarefa;

            return Page();
        }
    }
}