using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Apontamento
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly IApontamentoService _apontamentoService;

        public RemoverModel(IApontamentoService apontamentoService)
        {
            _apontamentoService = apontamentoService;
        }

        [BindProperty]
        public ApontamentoViewModel Apontamento { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                Apontamento = await _apontamentoService.ConsultarAsync(Token, id);

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _apontamentoService.RemoverAsync(Token, Apontamento.Id);

                return RedirectToPage("Listar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}