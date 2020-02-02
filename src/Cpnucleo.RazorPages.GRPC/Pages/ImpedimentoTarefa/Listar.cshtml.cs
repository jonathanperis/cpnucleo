using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.GRPC.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly IImpedimentoTarefaApiService _impedimentoTarefaApiService;

        public ListarModel(IClaimsManager claimsManager,
                                    IImpedimentoTarefaApiService impedimentoTarefaApiService)
            : base(claimsManager)
        {
            _impedimentoTarefaApiService = impedimentoTarefaApiService;
        }

        [BindProperty]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public IEnumerable<ImpedimentoTarefaViewModel> Lista { get; set; }

        public IActionResult OnGet(Guid idTarefa)
        {
            Lista = _impedimentoTarefaApiService.ListarPorTarefa(Token, idTarefa);

            ViewData["idTarefa"] = idTarefa;

            return Page();
        }
    }
}