using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
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
                var result = await _tarefaService.ConsultarAsync(Token, idTarefa);

                if (!result.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                    return Page();
                }

                Tarefa = result.response;

                var result2 = await _impedimentoService.ListarAsync(Token);

                if (!result2.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                    return Page();
                }

                SelectImpedimentos = new SelectList(result2.response, "Id", "Nome");

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
                    var result = await _tarefaService.ConsultarAsync(Token, ImpedimentoTarefa.IdTarefa);

                    if (!result.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                        return Page();
                    }

                    Tarefa = result.response;

                    var result2 = await _impedimentoService.ListarAsync(Token);

                    if (!result2.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                        return Page();
                    }

                    SelectImpedimentos = new SelectList(result2.response, "Id", "Nome");

                    return Page();
                }

                var result3 = await _impedimentoTarefaService.IncluirAsync(Token, ImpedimentoTarefa);

                if (!result3.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result3.code} - {result3.message}");
                    return Page();
                }                

                return RedirectToPage("Listar", new { idTarefa = ImpedimentoTarefa.IdTarefa });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}