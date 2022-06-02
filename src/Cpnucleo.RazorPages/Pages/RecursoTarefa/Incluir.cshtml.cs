namespace Cpnucleo.RazorPages.Pages.RecursoTarefa;

//[Authorize]
public class IncluirModel : PageBase
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

            var result = await _cpnucleoApiClient.ExecuteCommandAsync<OperationResult>("RecursoTarefa", "CreateRecursoTarefa", Token, new CreateRecursoTarefaCommand { IdTarefa = RecursoTarefa.IdTarefa, IdRecurso = RecursoTarefa.IdRecurso });

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
        var result = await _cpnucleoApiClient.ExecuteQueryAsync<GetTarefaViewModel>("Tarefa", "GetTarefa", Token, new GetTarefaQuery { Id = idTarefa });

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        Tarefa = result.Tarefa;

        var result2 = await _cpnucleoApiClient.ExecuteQueryAsync<ListRecursoViewModel>("Recurso", "ListRecurso", Token, new ListRecursoQuery { });

        if (result2.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectRecursos = new SelectList(result2.Recursos, "Id", "Nome");
    }
}
