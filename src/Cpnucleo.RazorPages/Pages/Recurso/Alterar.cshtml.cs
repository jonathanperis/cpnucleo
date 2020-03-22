using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Recurso
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

        public async Task<IActionResult> OnGet(Guid id)
        {
            Recurso = await _recursoApiService.ConsultarAsync(Token, id);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _recursoApiService.AlterarAsync(Token, Recurso);

            return RedirectToPage("Listar");
        }
    }
}