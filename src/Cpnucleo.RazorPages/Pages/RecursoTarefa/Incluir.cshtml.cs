namespace Cpnucleo.RazorPages.Pages.RecursoTarefa;

[Authorize]
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
            Tarefa = await _cpnucleoApiClient.GetAsync<TarefaDTO>("tarefa", Token, idTarefa);

            IEnumerable<RecursoProjetoDTO> result = await _cpnucleoApiClient.GetAsync<IEnumerable<RecursoProjetoDTO>>("recursoProjeto", "getByProjeto", Token, Tarefa.IdProjeto);
            SelectRecursos = new SelectList(result, "Recurso.Id", "Recurso.Nome");

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
                Tarefa = await _cpnucleoApiClient.GetAsync<TarefaDTO>("tarefa", Token, RecursoTarefa.IdTarefa);

                IEnumerable<RecursoProjetoDTO> result = await _cpnucleoApiClient.GetAsync<IEnumerable<RecursoProjetoDTO>>("recursoProjeto", "getByProjeto", Token, Tarefa.IdProjeto);
                SelectRecursos = new SelectList(result, "Recurso.Id", "Recurso.Nome");

                return Page();
            }

            await _cpnucleoApiClient.PostAsync<RecursoTarefaDTO>("recursoTarefa", Token, RecursoTarefa);

            return RedirectToPage("Listar", new { idTarefa = RecursoTarefa.IdTarefa });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
