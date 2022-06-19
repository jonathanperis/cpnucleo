namespace Cpnucleo.RazorPages.Pages.Tarefa;

[Authorize]
public class AlterarModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public AlterarModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public TarefaDTO Tarefa { get; set; }

    public SelectList SelectProjetos { get; set; }

    public SelectList SelectSistemas { get; set; }

    public SelectList SelectWorkflows { get; set; }

    public SelectList SelectTipoTarefas { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            await CarregarDados(id);

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(Tarefa.Id);

                return Page();
            }

            OperationResult result = await _cpnucleoApiClient.ExecuteCommandAsync<OperationResult>("Tarefa", "UpdateTarefa", new UpdateTarefaCommand(Tarefa.Id, Tarefa.Nome, Tarefa.DataInicio, Tarefa.DataTermino, Tarefa.QtdHoras, Tarefa.Detalhe, Tarefa.IdProjeto, Tarefa.IdWorkflow, Tarefa.IdRecurso, Tarefa.IdTipoTarefa));

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

    private async Task CarregarDados(Guid idTarefa)
    {
        GetTarefaViewModel result = await _cpnucleoApiClient.ExecuteQueryAsync<GetTarefaViewModel>("Tarefa", "GetTarefa", new GetTarefaQuery { Id = idTarefa });

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        Tarefa = result.Tarefa;

        ListProjetoViewModel result2 = await _cpnucleoApiClient.ExecuteQueryAsync<ListProjetoViewModel>("Projeto", "ListProjeto", new ListProjetoQuery { });

        if (result2.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectProjetos = new SelectList(result2.Projetos, "Id", "Nome");

        ListSistemaViewModel result3 = await _cpnucleoApiClient.ExecuteQueryAsync<ListSistemaViewModel>("Sistema", "ListSistema", new ListSistemaQuery { });

        if (result3.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectSistemas = new SelectList(result3.Sistemas, "Id", "Nome");

        ListWorkflowViewModel result4 = await _cpnucleoApiClient.ExecuteQueryAsync<ListWorkflowViewModel>("Workflow", "ListWorkflow", new ListWorkflowQuery { });

        if (result4.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectWorkflows = new SelectList(result4.Workflows, "Id", "Nome");

        ListTipoTarefaViewModel result5 = await _cpnucleoApiClient.ExecuteQueryAsync<ListTipoTarefaViewModel>("TipoTarefa", "ListTipoTarefa", new ListTipoTarefaQuery { });

        if (result5.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectTipoTarefas = new SelectList(result5.TipoTarefas, "Id", "Nome");
    }
}
