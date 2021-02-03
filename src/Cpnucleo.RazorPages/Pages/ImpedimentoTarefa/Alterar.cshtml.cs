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
    public class AlterarModel : PageBase
    {
        private readonly IImpedimentoTarefaService _impedimentoTarefaService;
        private readonly ICrudService<ImpedimentoViewModel> _impedimentoService;

        public AlterarModel(IImpedimentoTarefaService impedimentoTarefaService,
                            ICrudService<ImpedimentoViewModel> impedimentoService)
        {
            _impedimentoTarefaService = impedimentoTarefaService;
            _impedimentoService = impedimentoService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                ImpedimentoTarefa = await _impedimentoTarefaService.ConsultarAsync(Token, id);
                SelectImpedimentos = new SelectList(await _impedimentoService.ListarAsync(Token), "Id", "Nome");

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
                    SelectImpedimentos = new SelectList(await _impedimentoService.ListarAsync(Token), "Id", "Nome");

                    return Page();
                }

                await _impedimentoTarefaService.AlterarAsync(Token, ImpedimentoTarefa);

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