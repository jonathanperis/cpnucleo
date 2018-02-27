using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.Workflow;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace dotnet_cpnucleo_pages.Pages.Workflow
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IWorkflowRepository _workflowRepository;

        public ListarModel(IWorkflowRepository workflowRepository)
        {
            _workflowRepository = workflowRepository;
        }

        [BindProperty]
        public WorkflowItem Workflow { get; set; }

        [BindProperty]
        public IEnumerable<WorkflowItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _workflowRepository.Listar();

            return Page();
        }
    }
}