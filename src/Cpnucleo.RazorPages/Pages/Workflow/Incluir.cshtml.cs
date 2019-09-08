using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cpnucleo.RazorPages.Pages.Workflow
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IWorkflowAppService _workflowAppService;

        public IncluirModel(IWorkflowAppService workflowAppService) => _workflowAppService = workflowAppService;

        [BindProperty]
        public WorkflowViewModel Workflow { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            _workflowAppService.Incluir(Workflow);

            return RedirectToPage("Listar");
        }
    }
}