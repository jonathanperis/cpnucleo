using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Tarefa
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

        public async Task<IActionResult> OnGet()
        {
            Lista = _tarefaApiService.Listar(Token);

            return Page();
        }
    }
}