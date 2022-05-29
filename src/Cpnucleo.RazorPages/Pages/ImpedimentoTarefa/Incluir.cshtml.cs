namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa;

[Authorize]
public class IncluirModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public IncluirModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public ImpedimentoTarefaDTO ImpedimentoTarefa { get; set; }

    public TarefaDTO Tarefa { get; set; }

    public SelectList SelectImpedimentos { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid idTarefa)
    {
        try
        {
            Tarefa = await _cpnucleoApiClient.GetAsync<TarefaDTO>("tarefa", Token, idTarefa);

            IEnumerable<ImpedimentoDTO> result = await _cpnucleoApiClient.GetAsync<IEnumerable<ImpedimentoDTO>>("impedimento", Token);
            SelectImpedimentos = new SelectList(result, "Id", "Nome");

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
                Tarefa = await _cpnucleoApiClient.GetAsync<TarefaDTO>("tarefa", Token, ImpedimentoTarefa.IdTarefa);

                IEnumerable<ImpedimentoDTO> result = await _cpnucleoApiClient.GetAsync<IEnumerable<ImpedimentoDTO>>("impedimento", Token);
                SelectImpedimentos = new SelectList(result, "Id", "Nome");

                return Page();
            }

            await _cpnucleoApiClient.PostAsync<ImpedimentoTarefaDTO>("impedimentoTarefa", Token, ImpedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa = ImpedimentoTarefa.IdTarefa });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
