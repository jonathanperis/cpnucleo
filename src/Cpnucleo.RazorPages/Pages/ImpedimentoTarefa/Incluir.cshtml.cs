using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly IImpedimentoTarefaService _impedimentoTarefaService;
        private readonly ICrudService<ImpedimentoViewModel> _impedimentoService;
        private readonly ITarefaService _tarefaService;

        public IncluirModel(IImpedimentoTarefaService impedimentoTarefaService,
                            ICrudService<ImpedimentoViewModel> impedimentoService,
                            ITarefaService tarefaService)
        {
            _impedimentoTarefaService = impedimentoTarefaService;
            _impedimentoService = impedimentoService;
            _tarefaService = tarefaService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid idTarefa)
        {
            try
            {
                Tarefa = await _tarefaService.ConsultarAsync(Token, idTarefa);

                SelectImpedimentos = new SelectList(await _impedimentoService.ListarAsync(Token), "Id", "Nome");

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid idTarefa)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Tarefa = await _tarefaService.ConsultarAsync(Token, idTarefa);

                    SelectImpedimentos = new SelectList(await _impedimentoService.ListarAsync(Token), "Id", "Nome");

                    return Page();
                }

                await _impedimentoTarefaService.IncluirAsync(Token, ImpedimentoTarefa);

                return RedirectToPage("Listar", new { idTarefa });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}