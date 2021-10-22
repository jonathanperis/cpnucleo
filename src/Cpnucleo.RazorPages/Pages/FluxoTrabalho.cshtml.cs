namespace Cpnucleo.RazorPages.Pages;

[Authorize]
public class FluxoTrabalhoModel : PageBase
{
    private readonly ICpnucleoApiService _cpnucleoApiService;

    public FluxoTrabalhoModel(ICpnucleoApiService cpnucleoApiService)
    {
        _cpnucleoApiService = cpnucleoApiService;
    }

    public IEnumerable<WorkflowViewModel> Lista { get; set; }

    public IEnumerable<TarefaViewModel> ListaTarefas { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Lista = await _cpnucleoApiService.GetAsync<IEnumerable<WorkflowViewModel>>("workflow", Token);
            ListaTarefas = await _cpnucleoApiService.GetAsync<IEnumerable<TarefaViewModel>>("tarefa", Token, true);

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
                Lista = await _cpnucleoApiService.GetAsync<IEnumerable<WorkflowViewModel>>("workflow", Token);
                ListaTarefas = await _cpnucleoApiService.GetAsync<IEnumerable<TarefaViewModel>>("tarefa", Token, true);

                return Page();
            }

            WorkflowViewModel result3 = await _cpnucleoApiService.GetAsync<WorkflowViewModel>("workflow", Token, idWorkflow);
            await _cpnucleoApiService.PutAsync("tarefa", "putByWorkflow", Token, idTarefa, result3);

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
