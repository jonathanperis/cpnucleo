using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Luna.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IImpedimentoTarefaAppService _impedimentoTarefaAppService;

        public ListarModel(IImpedimentoTarefaAppService impedimentoTarefaAppService)
        {
            _impedimentoTarefaAppService = impedimentoTarefaAppService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public IEnumerable<ImpedimentoTarefaViewModel> Lista { get; set; }

        public IActionResult OnGet(Guid idTarefa)
        {
            Lista = _impedimentoTarefaAppService.ListarPorTarefa(idTarefa);

            ViewData["idTarefa"] = idTarefa;

            return Page();
        }
    }
}