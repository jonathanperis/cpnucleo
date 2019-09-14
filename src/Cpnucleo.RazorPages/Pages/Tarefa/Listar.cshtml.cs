using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Pages.Tarefa
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly ITarefaAppService _tarefaAppService;

        public ListarModel(ITarefaAppService tarefaAppService)
        {
            _tarefaAppService = tarefaAppService;
        }

        public TarefaViewModel Tarefa { get; set; }

        public IEnumerable<TarefaViewModel> Lista { get; set; }

        public IActionResult OnGet()
        {
            Lista = _tarefaAppService.Listar();

            return Page();
        }
    }
}