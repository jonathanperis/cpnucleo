using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.Workflow;
using Microsoft.AspNetCore.Authorization;

namespace dotnet_cpnucleo_pages.Pages.Workflow
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IWorkflowRepository _WorkflowRepository;

        public AlterarModel(IWorkflowRepository WorkflowRepository)
        {
            _WorkflowRepository = WorkflowRepository;
        }

        [BindProperty]
        public WorkflowItem Workflow { get; set; }

        public async Task<IActionResult> OnGetAsync(int idWorkflow)
        {
            Workflow = await _WorkflowRepository.Consultar(idWorkflow);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(WorkflowItem workflow)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _WorkflowRepository.Alterar(workflow);

            return RedirectToPage("Listar");
        }
    }
}