namespace Cpnucleo.RazorPages.Pages;

[Authorize]
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
            Lista = await _cpnucleoApiClient.GetAsync<IEnumerable<WorkflowDTO>>("workflow", Token);
            ListaTarefas = await _cpnucleoApiClient.GetAsync<IEnumerable<TarefaDTO>>("tarefa", Token, true);

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
                Lista = await _cpnucleoApiClient.GetAsync<IEnumerable<WorkflowDTO>>("workflow", Token);
                ListaTarefas = await _cpnucleoApiClient.GetAsync<IEnumerable<TarefaDTO>>("tarefa", Token, true);

                return Page();
            }

            WorkflowDTO result3 = await _cpnucleoApiClient.GetAsync<WorkflowDTO>("workflow", Token, idWorkflow);
            await _cpnucleoApiClient.PutAsync("tarefa", "putByWorkflow", Token, idTarefa, result3);

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
