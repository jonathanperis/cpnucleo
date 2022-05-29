namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa;

[Authorize]
public class AlterarModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public AlterarModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public ImpedimentoTarefaDTO ImpedimentoTarefa { get; set; }

    public SelectList SelectImpedimentos { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            ImpedimentoTarefa = await _cpnucleoApiClient.GetAsync<ImpedimentoTarefaDTO>("impedimentoTarefa", Token, id);

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
                ImpedimentoTarefa = await _cpnucleoApiClient.GetAsync<ImpedimentoTarefaDTO>("impedimentoTarefa", Token, ImpedimentoTarefa.Id);

                IEnumerable<ImpedimentoDTO> result = await _cpnucleoApiClient.GetAsync<IEnumerable<ImpedimentoDTO>>("impedimento", Token);
                SelectImpedimentos = new SelectList(result, "Id", "Nome");

                return Page();
            }

            await _cpnucleoApiClient.PutAsync("impedimentoTarefa", Token, ImpedimentoTarefa.Id, ImpedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa = ImpedimentoTarefa.IdTarefa });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
