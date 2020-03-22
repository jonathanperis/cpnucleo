using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Tarefa
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly ITarefaApiService _tarefaApiService;

        public ListarModel(IClaimsManager claimsManager,
                                    ITarefaApiService tarefaApiService)
            : base(claimsManager)
        {
            _tarefaApiService = tarefaApiService;
        }

        public TarefaViewModel Tarefa { get; set; }

        public IEnumerable<TarefaViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _tarefaApiService.ListarAsync(Token);

            return Page();
        }
    }
}