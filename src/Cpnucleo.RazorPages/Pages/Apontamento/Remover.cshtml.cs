using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Apontamento
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly IApontamentoApiService _apontamentoApiService;

        public RemoverModel(IClaimsManager claimsManager,
                                    IApontamentoApiService apontamentoApiService)
            : base(claimsManager)
        {
            _apontamentoApiService = apontamentoApiService;
        }

        [BindProperty]
        public ApontamentoViewModel Apontamento { get; set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            Apontamento = await _apontamentoApiService.ConsultarAsync(Token, id);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            await _apontamentoApiService.RemoverAsync(Token, Apontamento.Id);

            return RedirectToPage("Listar");
        }
    }
}