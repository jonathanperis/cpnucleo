namespace Cpnucleo.RazorPages.Pages.Tarefa;

[Authorize]
public class IncluirModel : PageBase
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
            IEnumerable<ProjetoDTO> result = await _cpnucleoApiClient.GetAsync<IEnumerable<ProjetoDTO>>("projeto", Token);
            SelectProjetos = new SelectList(result, "Id", "Nome");

            IEnumerable<SistemaDTO> result2 = await _cpnucleoApiClient.GetAsync<IEnumerable<SistemaDTO>>("sistema", Token);
            SelectSistemas = new SelectList(result2, "Id", "Nome");

            IEnumerable<WorkflowDTO> result3 = await _cpnucleoApiClient.GetAsync<IEnumerable<WorkflowDTO>>("workflow", Token);
            SelectWorkflows = new SelectList(result3, "Id", "Nome");

            IEnumerable<TipoTarefaDTO> result4 = await _cpnucleoApiClient.GetAsync<IEnumerable<TipoTarefaDTO>>("tipoTarefa", Token);
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
                IEnumerable<ProjetoDTO> result = await _cpnucleoApiClient.GetAsync<IEnumerable<ProjetoDTO>>("projeto", Token);
                SelectProjetos = new SelectList(result, "Id", "Nome");

                IEnumerable<SistemaDTO> result2 = await _cpnucleoApiClient.GetAsync<IEnumerable<SistemaDTO>>("sistema", Token);
                SelectSistemas = new SelectList(result2, "Id", "Nome");

                IEnumerable<WorkflowDTO> result3 = await _cpnucleoApiClient.GetAsync<IEnumerable<WorkflowDTO>>("workflow", Token);
                SelectWorkflows = new SelectList(result3, "Id", "Nome");

                IEnumerable<TipoTarefaDTO> result4 = await _cpnucleoApiClient.GetAsync<IEnumerable<TipoTarefaDTO>>("tipoTarefa", Token);
                SelectTipoTarefas = new SelectList(result4, "Id", "Nome");

                return Page();
            }

            await _cpnucleoApiClient.PostAsync<TarefaDTO>("tarefa", Token, Tarefa);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
