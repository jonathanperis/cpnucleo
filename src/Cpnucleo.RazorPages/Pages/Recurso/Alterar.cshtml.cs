using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Recurso
{
    [Authorize]
    public class AlterarModel : PageBase
    {
        private readonly IRecursoService _recursoService;

        public AlterarModel(IRecursoService recursoService)
        {
            _recursoService = recursoService;
        }

        [BindProperty]
        public RecursoViewModel Recurso { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                Recurso = await _recursoService.ConsultarAsync(Token, id);

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
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                await _recursoService.AlterarAsync(Token, Recurso);

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