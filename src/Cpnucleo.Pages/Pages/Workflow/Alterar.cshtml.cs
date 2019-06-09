using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Workflow
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IWorkflowRepository _workflowRepository;

        public AlterarModel(IWorkflowRepository workflowRepository) => _workflowRepository = workflowRepository;

        [BindProperty(SupportsGet = true)]
        public WorkflowItem Workflow { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Workflow = await _workflowRepository.Consultar(Workflow.IdWorkflow);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _workflowRepository.Alterar(Workflow);

            return RedirectToPage("Listar");
        }
    }
}