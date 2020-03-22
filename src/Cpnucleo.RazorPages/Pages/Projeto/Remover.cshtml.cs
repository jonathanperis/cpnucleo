using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Projeto
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly ICrudApiService<ProjetoViewModel> _projetoApiService;

        public RemoverModel(IClaimsManager claimsManager,
                                    ICrudApiService<ProjetoViewModel> projetoApiService)
            : base(claimsManager)
        {
            _projetoApiService = projetoApiService;
        }

        [BindProperty]
        public ProjetoViewModel Projeto { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Projeto = await _projetoApiService.ConsultarAsync(Token, id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _projetoApiService.RemoverAsync(Token, Projeto.Id);

            return RedirectToPage("Listar");
        }
    }
}