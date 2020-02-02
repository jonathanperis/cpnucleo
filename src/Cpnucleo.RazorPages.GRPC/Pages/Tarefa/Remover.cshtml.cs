using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cpnucleo.RazorPages.GRPC.Pages.Tarefa
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly ITarefaApiService _tarefaApiService;

        public RemoverModel(IClaimsManager claimsManager,
                                    ITarefaApiService tarefaApiService)
            : base(claimsManager)
        {
            _tarefaApiService = tarefaApiService;
        }

        [BindProperty]
        public TarefaViewModel Tarefa { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Tarefa = _tarefaApiService.Consultar(Token, id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _tarefaApiService.Remover(Token, Tarefa.Id);

            return RedirectToPage("Listar");
        }
    }
}