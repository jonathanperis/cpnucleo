using Cpnucleo.Shared.Queries.ListWorkflow;

namespace Cpnucleo.RazorPages.Pages.Workflow;

[Authorize]
public class ListarModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public ListarModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    public WorkflowDTO Workflow { get; set; }

    public IEnumerable<WorkflowDTO> Lista { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            ListWorkflowViewModel result = await _cpnucleoApiClient.ExecuteQueryAsync<ListWorkflowViewModel>("Workflow", "ListWorkflow", new ListWorkflowQuery());

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return Page();
            }

            Lista = result.Workflows;

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
