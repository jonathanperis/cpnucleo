using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Tarefa
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly ITarefaGrpcService _tarefaGrpcService;

        public RemoverModel(ITarefaGrpcService tarefaGrpcService)
        {
            _tarefaGrpcService = tarefaGrpcService;
        }

        [BindProperty]
        public TarefaViewModel Tarefa { get; set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            Tarefa = await _tarefaGrpcService.ConsultarAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            await _tarefaGrpcService.RemoverAsync(Tarefa.Id);

            return RedirectToPage("Listar");
        }
    }
}