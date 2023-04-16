using Cpnucleo.Shared.Commands.CreateRecursoTarefa;
using Cpnucleo.Shared.Queries.GetTarefa;
using Cpnucleo.Shared.Queries.ListRecurso;

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
    public RecursoTarefaDto RecursoTarefa { get; set; }

    public TarefaDto Tarefa { get; set; }

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

            var result = await _cpnucleoApiClient.ExecuteAsync<OperationResult>("RecursoTarefa", "CreateRecursoTarefa", new CreateRecursoTarefaCommand(RecursoTarefa.IdRecurso, RecursoTarefa.IdTarefa));

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
        var result = await _cpnucleoApiClient.ExecuteAsync<GetTarefaViewModel>("Tarefa", "GetTarefa", new GetTarefaQuery(idTarefa));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        Tarefa = result.Tarefa;

        var result2 = await _cpnucleoApiClient.ExecuteAsync<ListRecursoViewModel>("Recurso", "ListRecurso", new ListRecursoQuery());

        if (result2.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectRecursos = new SelectList(result2.Recursos, "Id", "Nome");
    }
}
