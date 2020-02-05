using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Tarefa
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly ITarefaGrpcService _tarefaGrpcService;

        public ListarModel(ITarefaGrpcService tarefaGrpcService)
        {
            _tarefaGrpcService = tarefaGrpcService;
        }

        public TarefaViewModel Tarefa { get; set; }

        public IEnumerable<TarefaViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Lista = await _tarefaGrpcService.ListarAsync();

            return Page();
        }
    }
}