using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.RecursoProjeto
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly IRecursoProjetoApiService _recursoProjetoApiService;

        public RemoverModel(IClaimsManager claimsManager,
                                    IRecursoProjetoApiService recursoProjetoApiService)
            : base(claimsManager)
        {
            _recursoProjetoApiService = recursoProjetoApiService;
        }

        [BindProperty]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            RecursoProjeto = await _recursoProjetoApiService.ConsultarAsync(Token, id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _recursoProjetoApiService.RemoverAsync(Token, RecursoProjeto.Id);

            return RedirectToPage("Listar", new { idProjeto = RecursoProjeto.IdProjeto });
        }
    }
}