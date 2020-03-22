using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Impedimento
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly ICrudGrpcService<ImpedimentoViewModel> _impedimentoGrpcService;

        public AlterarModel(ICrudGrpcService<ImpedimentoViewModel> impedimentoGrpcService)
        {
            _impedimentoGrpcService = impedimentoGrpcService;
        }

        [BindProperty]
        public ImpedimentoViewModel Impedimento { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Impedimento = await _impedimentoGrpcService.ConsultarAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _impedimentoGrpcService.AlterarAsync(Impedimento);

            return RedirectToPage("Listar");
        }
    }
}