using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Cpnucleo.RazorPages.Pages.RecursoProjeto
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IRecursoProjetoAppService _recursoProjetoAppService;
        private readonly IRecursoAppService _recursoAppService;
        private readonly ICrudAppService<ProjetoViewModel> _projetoAppService;

        public IncluirModel(IRecursoProjetoAppService recursoProjetoAppService,
                                        IRecursoAppService recursoAppService,
                                        ICrudAppService<ProjetoViewModel> projetoAppService)
        {
            _recursoProjetoAppService = recursoProjetoAppService;
            _recursoAppService = recursoAppService;
            _projetoAppService = projetoAppService;
        }

        [BindProperty]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }

        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectRecursos { get; set; }

        public IActionResult OnGet(Guid idProjeto)
        {
            Projeto = _projetoAppService.Consultar(idProjeto);
            SelectRecursos = new SelectList(_recursoAppService.Listar(), "Id", "Nome");

            return Page();
        }

        public IActionResult OnPost(Guid idProjeto)
        {
            if (!ModelState.IsValid)
            {
                Projeto = _projetoAppService.Consultar(idProjeto);
                SelectRecursos = new SelectList(_recursoAppService.Listar(), "Id", "Nome");

                return Page();
            }

            _recursoProjetoAppService.Incluir(RecursoProjeto);

            return RedirectToPage("Listar", new { idProjeto });
        }
    }
}