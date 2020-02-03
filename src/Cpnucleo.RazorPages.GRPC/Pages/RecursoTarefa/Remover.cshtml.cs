using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.RecursoTarefa
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly IRecursoTarefaApiService _recursoTarefaApiService;

        public RemoverModel(IClaimsManager claimsManager,
                                    IRecursoTarefaApiService recursoTarefaApiService)
            : base(claimsManager)
        {
            _recursoTarefaApiService = recursoTarefaApiService;
        }

        [BindProperty]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            RecursoTarefa = _recursoTarefaApiService.Consultar(Token, id);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            _recursoTarefaApiService.Remover(Token, RecursoTarefa.Id);

            return RedirectToPage("Listar", new { idTarefa = RecursoTarefa.IdTarefa });
        }
    }
}