using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.Recurso;
using Cpnucleo.Shared.Queries.Tarefa;

namespace Cpnucleo.RazorPages.Pages.RecursoTarefa;

[Authorize]
public class IncluirModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public IncluirModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public RecursoTarefaDTO RecursoTarefa { get; set; }

    public TarefaDTO Tarefa { get; set; }

    public SelectList SelectRecursos { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid idTarefa)
    {
        try
        {
            await CarregarDados(idTarefa);

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
                await CarregarDados(RecursoTarefa.IdTarefa);

                return Page();
            }

            OperationResult result = await _cpnucleoApiClient.ExecuteCommandAsync<OperationResult>("RecursoTarefa", "CreateRecursoTarefa", new CreateRecursoTarefaCommand(Guid.Empty, RecursoTarefa.IdRecurso, RecursoTarefa.IdTarefa));

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return Page();
            }

            return RedirectToPage("Listar", new { idTarefa = RecursoTarefa.IdTarefa });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    private async Task CarregarDados(Guid idTarefa)
    {
        GetTarefaViewModel result = await _cpnucleoApiClient.ExecuteQueryAsync<GetTarefaViewModel>("Tarefa", "GetTarefa", new GetTarefaQuery(idTarefa));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        Tarefa = result.Tarefa;

        ListRecursoViewModel result2 = await _cpnucleoApiClient.ExecuteQueryAsync<ListRecursoViewModel>("Recurso", "ListRecurso", new ListRecursoQuery());

        if (result2.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectRecursos = new SelectList(result2.Recursos, "Id", "Nome");
    }
}
