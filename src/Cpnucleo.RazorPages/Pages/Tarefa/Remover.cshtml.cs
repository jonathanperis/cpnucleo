using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Tarefa
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

        public async Task<IActionResult> OnGet(Guid id)
        {
            Tarefa = await _tarefaApiService.ConsultarAsync(Token, id);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            await _tarefaApiService.RemoverAsync(Token, Tarefa.Id);

            return RedirectToPage("Listar");
        }
    }
}