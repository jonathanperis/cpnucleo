namespace Cpnucleo.RazorPages.Pages.Tarefa;

//[Authorize]
public class RemoverModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public TarefaDTO Tarefa { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            Tarefa = await _cpnucleoApiClient.GetAsync<TarefaDTO>("tarefa", Token, id);

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

                return Page();
            }

            await _cpnucleoApiClient.DeleteAsync("tarefa", Token, Tarefa.Id);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
