namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa;

//[Authorize]
public class RemoverModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public ImpedimentoTarefaDTO ImpedimentoTarefa { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            ImpedimentoTarefa = await _cpnucleoApiClient.GetAsync<ImpedimentoTarefaDTO>("impedimentoTarefa", Token, id);

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

                return Page();
            }

            await _cpnucleoApiClient.DeleteAsync("impedimentoTarefa", Token, ImpedimentoTarefa.Id);

            return RedirectToPage("Listar", new { idTarefa = ImpedimentoTarefa.IdTarefa });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
