using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cpnucleo.RazorPages.GRPC.Pages.Recurso
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly IRecursoApiService _recursoApiService;

        public RemoverModel(IClaimsManager claimsManager,
                                    IRecursoApiService recursoApiService)
            : base(claimsManager)
        {
            _recursoApiService = recursoApiService;
        }

        [BindProperty]
        public RecursoViewModel Recurso { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Recurso = _recursoApiService.Consultar(Token, id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _recursoApiService.Remover(Token, Recurso.Id);

            return RedirectToPage("Listar");
        }
    }
}