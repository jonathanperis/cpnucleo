using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cpnucleo.RazorPages.Luna.Pages.RecursoTarefa
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

        public IActionResult OnGet(Guid id)
        {
            RecursoTarefa = _recursoTarefaApiService.Consultar(Token, id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _recursoTarefaApiService.Remover(Token, RecursoTarefa.Id);

            return RedirectToPage("Listar", new { idTarefa = RecursoTarefa.IdTarefa });
        }
    }
}