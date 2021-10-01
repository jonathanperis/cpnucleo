namespace Cpnucleo.RazorPages.Pages.Tarefa;

[Authorize]
public class IncluirModel : PageBase
{
    private readonly ICpnucleoApiService _cpnucleoApiService;

    public IncluirModel(ICpnucleoApiService cpnucleoApiService)
    {
        _cpnucleoApiService = cpnucleoApiService;
    }

    [BindProperty]
    public TarefaViewModel Tarefa { get; set; }

    public SelectList SelectProjetos { get; set; }

    public SelectList SelectSistemas { get; set; }

    public SelectList SelectWorkflows { get; set; }

    public SelectList SelectTipoTarefas { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            IEnumerable<ProjetoViewModel> result = await _cpnucleoApiService.GetAsync<IEnumerable<ProjetoViewModel>>("projeto", Token);
            SelectProjetos = new SelectList(result, "Id", "Nome");

            IEnumerable<SistemaViewModel> result2 = await _cpnucleoApiService.GetAsync<IEnumerable<SistemaViewModel>>("sistema", Token);
            SelectSistemas = new SelectList(result2, "Id", "Nome");

            IEnumerable<WorkflowViewModel> result3 = await _cpnucleoApiService.GetAsync<IEnumerable<WorkflowViewModel>>("workflow", Token);
            SelectWorkflows = new SelectList(result3, "Id", "Nome");

            IEnumerable<TipoTarefaViewModel> result4 = await _cpnucleoApiService.GetAsync<IEnumerable<TipoTarefaViewModel>>("tipoTarefa", Token);
            SelectTipoTarefas = new SelectList(result4, "Id", "Nome");

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
                IEnumerable<ProjetoViewModel> result = await _cpnucleoApiService.GetAsync<IEnumerable<ProjetoViewModel>>("projeto", Token);
                SelectProjetos = new SelectList(result, "Id", "Nome");

                IEnumerable<SistemaViewModel> result2 = await _cpnucleoApiService.GetAsync<IEnumerable<SistemaViewModel>>("sistema", Token);
                SelectSistemas = new SelectList(result2, "Id", "Nome");

                IEnumerable<WorkflowViewModel> result3 = await _cpnucleoApiService.GetAsync<IEnumerable<WorkflowViewModel>>("workflow", Token);
                SelectWorkflows = new SelectList(result3, "Id", "Nome");

                IEnumerable<TipoTarefaViewModel> result4 = await _cpnucleoApiService.GetAsync<IEnumerable<TipoTarefaViewModel>>("tipoTarefa", Token);
                SelectTipoTarefas = new SelectList(result4, "Id", "Nome");

                return Page();
            }

            await _cpnucleoApiService.PostAsync<TarefaViewModel>("tarefa", Token, Tarefa);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
