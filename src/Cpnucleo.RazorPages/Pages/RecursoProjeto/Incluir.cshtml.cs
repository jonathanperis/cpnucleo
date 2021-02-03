using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.RecursoProjeto
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly IRecursoProjetoService _recursoProjetoService;
        private readonly IRecursoService _recursoService;
        private readonly ICrudService<ProjetoViewModel> _projetoService;

        public IncluirModel(IRecursoProjetoService recursoProjetoService,
                            IRecursoService recursoService,
                            ICrudService<ProjetoViewModel> projetoService)
        {
            _recursoProjetoService = recursoProjetoService;
            _recursoService = recursoService;
            _projetoService = projetoService;
        }

        [BindProperty]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }

        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid idProjeto)
        {
            try
            {
                Projeto = await _projetoService.ConsultarAsync(Token, idProjeto);
                SelectRecursos = new SelectList(await _recursoService.ListarAsync(Token), "Id", "Nome");

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid idProjeto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Projeto = await _projetoService.ConsultarAsync(Token, idProjeto);
                    SelectRecursos = new SelectList(await _recursoService.ListarAsync(Token), "Id", "Nome");

                    return Page();
                }

                await _recursoProjetoService.IncluirAsync(Token, RecursoProjeto);

                return RedirectToPage("Listar", new { idProjeto });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}