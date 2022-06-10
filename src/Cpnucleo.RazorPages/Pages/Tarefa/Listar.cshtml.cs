namespace Cpnucleo.RazorPages.Pages.Tarefa;

[Authorize]
public class ListarModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public ListarModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    public TarefaDTO Tarefa { get; set; }

    public IEnumerable<TarefaDTO> Lista { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            ListTarefaViewModel result = await _cpnucleoApiClient.ExecuteQueryAsync<ListTarefaViewModel>("Tarefa", "ListTarefa", new ListTarefaQuery { GetDependencies = true });

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return Page();
            }

            Lista = result.Tarefas;

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
