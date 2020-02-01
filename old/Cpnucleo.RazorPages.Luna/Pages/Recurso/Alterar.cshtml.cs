using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cpnucleo.RazorPages.Luna.Pages.Recurso
{
    [Authorize]
    public class AlterarModel : PageBase
    {
        private readonly IRecursoApiService _recursoApiService;

        public AlterarModel(IClaimsManager claimsManager,
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _recursoApiService.Alterar(Token, Recurso);

            return RedirectToPage("Listar");
        }
    }
}