namespace Cpnucleo.RazorPages.Pages;

//[Authorize]
public class FluxoTrabalhoModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public FluxoTrabalhoModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    public IEnumerable<WorkflowDTO> Lista { get; set; }

    public IEnumerable<TarefaDTO> ListaTarefas { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            await CarregarDados();

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync(Guid idTarefa, Guid idWorkflow)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados();

                return Page();
            }

            var result = await _cpnucleoApiClient.ExecuteCommandAsync<OperationResult>("Tarefa", "UpdateTarefaByWorkflow", Token, new UpdateTarefaByWorkflowCommand { Id = idTarefa, IdWorkflow = idWorkflow });

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return Page();
            }

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    private async Task CarregarDados()
    {
        var result = await _cpnucleoApiClient.ExecuteQueryAsync<ListWorkflowViewModel>("Workflow", "ListWorkflow", Token, new ListWorkflowQuery { });

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        Lista = result.Workflows;

        var result2 = await _cpnucleoApiClient.ExecuteQueryAsync<ListTarefaViewModel>("Tarefa", "ListTarefa", Token, new ListTarefaQuery { GetDependencies = true });

        if (result2.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ListaTarefas = result2.Tarefas;
    }
}
