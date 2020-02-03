using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.GRPC.Pages.Projeto
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly IProjetoApiService _projetoApiService;

        public ListarModel(IClaimsManager claimsManager,
                                    IProjetoApiService projetoApiService)
            : base(claimsManager)
        {
            _projetoApiService = projetoApiService;
        }

        public ProjetoViewModel Projeto { get; set; }

        public IEnumerable<ProjetoViewModel> Lista { get; set; }

        public IActionResult OnGet()
        {
            Lista = _projetoApiService.Listar(Token);

            return Page();
        }
    }
}