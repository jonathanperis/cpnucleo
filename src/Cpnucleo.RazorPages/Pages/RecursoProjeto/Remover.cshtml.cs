using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.RecursoProjeto
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly IRecursoProjetoService _recursoProjetoService;

        public RemoverModel(IRecursoProjetoService recursoProjetoService)
        {
            _recursoProjetoService = recursoProjetoService;
        }

        [BindProperty]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                RecursoProjeto = await _recursoProjetoService.ConsultarAsync(Token, id);

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
                await _recursoProjetoService.RemoverAsync(Token, RecursoProjeto.Id);

                return RedirectToPage("Listar", new { idProjeto = RecursoProjeto.IdProjeto });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}