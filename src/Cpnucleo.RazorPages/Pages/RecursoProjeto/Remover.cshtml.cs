namespace Cpnucleo.RazorPages.Pages.RecursoProjeto;

[Authorize]
public class RemoverModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public RecursoProjetoDTO RecursoProjeto { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            RecursoProjeto = await _cpnucleoApiClient.GetAsync<RecursoProjetoDTO>("recursoProjeto", Token, id);

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
                RecursoProjeto = await _cpnucleoApiClient.GetAsync<RecursoProjetoDTO>("recursoProjeto", Token, RecursoProjeto.Id);

                return Page();
            }

            await _cpnucleoApiClient.DeleteAsync("recursoProjeto", Token, RecursoProjeto.Id);

            return RedirectToPage("Listar", new { idProjeto = RecursoProjeto.IdProjeto });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
