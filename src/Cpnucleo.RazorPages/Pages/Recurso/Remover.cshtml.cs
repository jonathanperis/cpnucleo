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

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Recurso = await _recursoApiService.ConsultarAsync(Token, id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _recursoApiService.RemoverAsync(Token, Recurso.Id);

            return RedirectToPage("Listar");
        }
    }
}