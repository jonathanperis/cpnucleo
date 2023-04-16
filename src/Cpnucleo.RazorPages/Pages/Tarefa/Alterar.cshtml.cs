using Cpnucleo.Shared.Commands.UpdateTarefa;
using Cpnucleo.Shared.Queries.GetTarefa;
using Cpnucleo.Shared.Queries.ListProjeto;
using Cpnucleo.Shared.Queries.ListSistema;
using Cpnucleo.Shared.Queries.ListTipoTarefa;
using Cpnucleo.Shared.Queries.ListWorkflow;

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

            var result = await _cpnucleoApiClient.ExecuteAsync<OperationResult>("Tarefa", "UpdateTarefa", new UpdateTarefaCommand(Tarefa.Id, Tarefa.Nome, Tarefa.DataInicio, Tarefa.DataTermino, Tarefa.QtdHoras, Tarefa.Detalhe, Tarefa.IdProjeto, Tarefa.IdWorkflow, Tarefa.IdRecurso, Tarefa.IdTipoTarefa));

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
        var result = await _cpnucleoApiClient.ExecuteAsync<GetTarefaViewModel>("Tarefa", "GetTarefa", new GetTarefaQuery(idTarefa));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        Tarefa = result.Tarefa;

        var result2 = await _cpnucleoApiClient.ExecuteAsync<ListProjetoViewModel>("Projeto", "ListProjeto", new ListProjetoQuery());

        if (result2.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectProjetos = new SelectList(result2.Projetos, "Id", "Nome");

        var result3 = await _cpnucleoApiClient.ExecuteAsync<ListSistemaViewModel>("Sistema", "ListSistema", new ListSistemaQuery());

        if (result3.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectSistemas = new SelectList(result3.Sistemas, "Id", "Nome");

        var result4 = await _cpnucleoApiClient.ExecuteAsync<ListWorkflowViewModel>("Workflow", "ListWorkflow", new ListWorkflowQuery());

        if (result4.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectWorkflows = new SelectList(result4.Workflows, "Id", "Nome");

        var result5 = await _cpnucleoApiClient.ExecuteAsync<ListTipoTarefaViewModel>("TipoTarefa", "ListTipoTarefa", new ListTipoTarefaQuery());

        if (result5.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectTipoTarefas = new SelectList(result5.TipoTarefas, "Id", "Nome");
    }
}
