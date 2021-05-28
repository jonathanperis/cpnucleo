using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Sistema
{
    //[Authorize]
    public class ListarModel : PageBase
    {
        private readonly ISistemaService _sistemaService;

        public ListarModel(ISistemaService sistemaService)
        {
            _sistemaService = sistemaService;
        }

        public SistemaViewModel Sistema { get; set; }

        public IEnumerable<SistemaViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ListSistemaResponse response = await _sistemaService.AllAsync(Token, new ListSistemaQuery { GetDependencies = true });

                Lista = response.Sistemas;

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}