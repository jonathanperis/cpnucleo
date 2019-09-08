using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Pages.Workflow
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IWorkflowAppService _workflowAppService;

        public ListarModel(IWorkflowAppService workflowAppService) => _workflowAppService = workflowAppService;

        public WorkflowViewModel Workflow { get; set; }

        public IEnumerable<WorkflowViewModel> Lista { get; set; }

        public IActionResult OnGet()
        {
            Lista = _workflowAppService.Listar();

            return Page();
        }
    }
}