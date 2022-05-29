namespace Cpnucleo.RazorPages.Pages.RecursoTarefa;

//[Authorize]
public class RemoverModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public RecursoTarefaDTO RecursoTarefa { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            RecursoTarefa = await _cpnucleoApiClient.GetAsync<RecursoTarefaDTO>("recursoTarefa", Token, id);

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
                RecursoTarefa = await _cpnucleoApiClient.GetAsync<RecursoTarefaDTO>("recursoTarefa", Token, RecursoTarefa.Id);

                return Page();
            }

            await _cpnucleoApiClient.DeleteAsync("recursoTarefa", Token, RecursoTarefa.Id);

            return RedirectToPage("Listar", new { idTarefa = RecursoTarefa.IdTarefa });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
