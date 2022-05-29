namespace Cpnucleo.RazorPages.Pages.Tarefa;

//[Authorize]
public class AlterarModel : PageBase
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
            Tarefa = await _cpnucleoApiClient.GetAsync<TarefaDTO>("tarefa", Token, id);

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
                Tarefa = await _cpnucleoApiClient.GetAsync<TarefaDTO>("tarefa", Token, Tarefa.Id);

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

            await _cpnucleoApiClient.PutAsync("tarefa", Token, Tarefa.Id, Tarefa);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
