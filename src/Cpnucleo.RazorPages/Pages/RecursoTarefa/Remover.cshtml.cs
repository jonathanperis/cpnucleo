namespace Cpnucleo.RazorPages.Pages.RecursoTarefa;

//[Authorize]
public class RemoverModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public RecursoTarefaDTO RecursoTarefa { get; set; }

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
                await CarregarDados(RecursoTarefa.Id);

                return Page();
            }

            var result = await _cpnucleoApiClient.ExecuteCommandAsync<OperationResult>("RecursoTarefa", "RemoveRecursoTarefa", Token, new RemoveRecursoTarefaCommand { Id = RecursoTarefa.Id });

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

    private async Task CarregarDados(Guid idRecursoTarefa)
    {
        var result = await _cpnucleoApiClient.ExecuteQueryAsync<GetRecursoTarefaViewModel>("RecursoTarefa", "GetRecursoTarefa", Token, new GetRecursoTarefaQuery { Id = idRecursoTarefa });

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        RecursoTarefa = result.RecursoTarefa;
    }
}
