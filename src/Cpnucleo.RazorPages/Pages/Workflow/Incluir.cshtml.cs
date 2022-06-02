namespace Cpnucleo.RazorPages.Pages.Workflow;

//[Authorize]
public class IncluirModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public IncluirModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public WorkflowDTO Workflow { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _cpnucleoApiClient.ExecuteCommandAsync<OperationResult>("Workflow", "CreateWorkflow", Token, new CreateWorkflowCommand { Nome = Workflow.Nome, Ordem = Workflow.Ordem });

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return Page();
            }

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
