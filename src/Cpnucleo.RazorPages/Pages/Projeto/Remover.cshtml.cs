namespace Cpnucleo.RazorPages.Pages.Projeto;

[Authorize]
public class RemoverModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public ProjetoDTO Projeto { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            Projeto = await _cpnucleoApiClient.GetAsync<ProjetoDTO>("projeto", Token, id);

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
                Projeto = await _cpnucleoApiClient.GetAsync<ProjetoDTO>("projeto", Token, Projeto.Id);

                return Page();
            }

            await _cpnucleoApiClient.DeleteAsync("projeto", Token, Projeto.Id);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
