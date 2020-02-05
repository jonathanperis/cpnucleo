using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.RecursoTarefa
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IRecursoTarefaGrpcService _recursoTarefaGrpcService;

        public ListarModel(IRecursoTarefaGrpcService recursoTarefaGrpcService)   
        {
            _recursoTarefaGrpcService = recursoTarefaGrpcService;
        }

        [BindProperty]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }

        public IEnumerable<RecursoTarefaViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGet(Guid idTarefa)
        {
            Lista = await _recursoTarefaGrpcService.ListarPorTarefaAsync(idTarefa);

            ViewData["idTarefa"] = idTarefa;

            return Page();
        }
    }
}