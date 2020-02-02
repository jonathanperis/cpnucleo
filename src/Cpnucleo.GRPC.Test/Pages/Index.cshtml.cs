using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Cpnucleo.GRPC.Test.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ISistemaGrpcService _sistemaGrpcService;

        public IndexModel(ILogger<IndexModel> logger, ISistemaGrpcService sistemaGrpcService)
        {
            _logger = logger;
            _sistemaGrpcService = sistemaGrpcService;
        }

        public async void OnGet()
        {
            //SistemaViewModel sistema = new SistemaViewModel
            //{
            //    Nome = "Cpnucleo",
            //    Descricao = "Sistemão da porra."
            //};

            //await _sistemaGrpcService.IncluirAsync(sistema);

            IEnumerable<SistemaViewModel> result = await _sistemaGrpcService.ListarAsync();
        }
    }
}
