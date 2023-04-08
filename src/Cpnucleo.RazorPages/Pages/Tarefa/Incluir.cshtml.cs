using Cpnucleo.Shared.Queries.ListProjeto;
using Cpnucleo.Shared.Queries.ListSistema;
using Cpnucleo.Shared.Queries.ListTipoTarefa;
using Cpnucleo.Shared.Queries.ListWorkflow;

namespace Cpnucleo.RazorPages.Pages.Tarefa;

[Authorize]
public class IncluirModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public IncluirModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public TarefaDTO Tarefa { get; set; }

    public SelectList SelectProjetos { get; set; }

    public SelectList SelectSistemas { get; set; }

    public SelectList SelectWorkflows { get; set; }

    public SelectList SelectTipoTarefas { get; set; }

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

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados();

                return Page();
            }

            OperationResult result = await _cpnucleoApiClient.ExecuteCommandAsync<OperationResult>("Tarefa", "CreateTarefa", new CreateTarefaCommand(Guid.Empty, Tarefa.Nome, Tarefa.DataInicio, Tarefa.DataTermino, Tarefa.QtdHoras, Tarefa.Detalhe, Tarefa.IdProjeto, Tarefa.IdWorkflow, Tarefa.IdRecurso, Tarefa.IdTipoTarefa));

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

    private async Task CarregarDados()
    {
        ListProjetoViewModel result = await _cpnucleoApiClient.ExecuteQueryAsync<ListProjetoViewModel>("Projeto", "ListProjeto", new ListProjetoQuery());

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectProjetos = new SelectList(result.Projetos, "Id", "Nome");

        ListSistemaViewModel result2 = await _cpnucleoApiClient.ExecuteQueryAsync<ListSistemaViewModel>("Sistema", "ListSistema", new ListSistemaQuery());

        if (result2.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectSistemas = new SelectList(result2.Sistemas, "Id", "Nome");

        ListWorkflowViewModel result3 = await _cpnucleoApiClient.ExecuteQueryAsync<ListWorkflowViewModel>("Workflow", "ListWorkflow", new ListWorkflowQuery());

        if (result3.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectWorkflows = new SelectList(result3.Workflows, "Id", "Nome");

        ListTipoTarefaViewModel result4 = await _cpnucleoApiClient.ExecuteQueryAsync<ListTipoTarefaViewModel>("TipoTarefa", "ListTipoTarefa", new ListTipoTarefaQuery());

        if (result4.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectTipoTarefas = new SelectList(result4.TipoTarefas, "Id", "Nome");
    }
}
