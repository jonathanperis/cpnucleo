namespace Cpnucleo.RazorPages.Pages.Apontamento;

//[Authorize]
public class RemoverModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public ApontamentoDTO Apontamento { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            Apontamento = await _cpnucleoApiClient.GetAsync<ApontamentoDTO>("apontamento", Token, id);

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
                Apontamento = await _cpnucleoApiClient.GetAsync<ApontamentoDTO>("apontamento", Token, Apontamento.Id);

                return Page();
            }

            await _cpnucleoApiClient.DeleteAsync("apontamento", Token, Apontamento.Id);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
