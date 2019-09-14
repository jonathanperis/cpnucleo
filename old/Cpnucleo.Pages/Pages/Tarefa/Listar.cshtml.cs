using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Tarefa
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly ITarefaRepository _tarefaRepository;

        public ListarModel(ITarefaRepository tarefaRepository) => _tarefaRepository = tarefaRepository;

        public TarefaModel Tarefa { get; set; }

        public IEnumerable<TarefaModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _tarefaRepository.ListarAsync();

            return Page();
        }
    }
}