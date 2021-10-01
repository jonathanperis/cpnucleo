namespace Cpnucleo.RazorPages.Pages.RecursoTarefa;

[Authorize]
public class RemoverModel : PageBase
{
    private readonly ICpnucleoApiService _cpnucleoApiService;

    public RemoverModel(ICpnucleoApiService cpnucleoApiService)
    {
        _cpnucleoApiService = cpnucleoApiService;
    }

    [BindProperty]
    public RecursoTarefaViewModel RecursoTarefa { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            RecursoTarefa = await _cpnucleoApiService.GetAsync<RecursoTarefaViewModel>("recursoTarefa", Token, id);

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
                RecursoTarefa = await _cpnucleoApiService.GetAsync<RecursoTarefaViewModel>("recursoTarefa", Token, RecursoTarefa.Id);

                return Page();
            }

            await _cpnucleoApiService.DeleteAsync("recursoTarefa", Token, RecursoTarefa.Id);

            return RedirectToPage("Listar", new { idTarefa = RecursoTarefa.IdTarefa });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
