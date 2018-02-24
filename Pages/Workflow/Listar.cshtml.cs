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
        private readonly IWorkflowRepository _WorkflowRepository;

        public ListarModel(IWorkflowRepository WorkflowRepository)
        {
            _WorkflowRepository = WorkflowRepository;
        }

        [BindProperty]
        public WorkflowItem Workflow { get; set; }

        [BindProperty]
        public IList<WorkflowItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _WorkflowRepository.Listar();

            return Page();
        }
    }
}