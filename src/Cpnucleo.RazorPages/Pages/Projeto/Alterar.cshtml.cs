using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Projeto
{
    [Authorize]
    public class AlterarModel : PageBase
    {
        private readonly ICrudService<ProjetoViewModel> _projetoService;
        private readonly ICrudService<SistemaViewModel> _sistemaService;

        public AlterarModel(ICrudService<ProjetoViewModel> projetoService,
                            ICrudService<SistemaViewModel> sistemaService)
        {
            _projetoService = projetoService;
            _sistemaService = sistemaService;
        }

        [BindProperty]
        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectSistemas { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                Projeto = await _projetoService.ConsultarAsync(Token, id);
                SelectSistemas = new SelectList(await _sistemaService.ListarAsync(Token), "Id", "Nome");

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
                    SelectSistemas = new SelectList(await _sistemaService.ListarAsync(Token), "Id", "Nome");

                    return Page();
                }

                await _projetoService.AlterarAsync(Token, Projeto);

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