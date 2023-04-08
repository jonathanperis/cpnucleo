using Cpnucleo.Shared.Commands.UpdateTarefaByWorkflow;
using Cpnucleo.Shared.Queries.ListTarefa;
using Cpnucleo.Shared.Queries.ListWorkflow;

namespace Cpnucleo.RazorPages.Pages.Apontamento;

[Authorize]
public class FluxoTrabalhoModel : PageModel
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

            OperationResult result = await _cpnucleoApiClient.ExecuteAsync<OperationResult>("Tarefa", "UpdateTarefaByWorkflow", new UpdateTarefaByWorkflowCommand(idTarefa, idWorkflow));

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
        ListWorkflowViewModel result = await _cpnucleoApiClient.ExecuteAsync<ListWorkflowViewModel>("Workflow", "ListWorkflow", new ListWorkflowQuery());

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        Lista = result.Workflows;

        ListTarefaViewModel result2 = await _cpnucleoApiClient.ExecuteAsync<ListTarefaViewModel>("Tarefa", "ListTarefa", new ListTarefaQuery(true));

        if (result2.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ListaTarefas = result2.Tarefas;
    }
}
