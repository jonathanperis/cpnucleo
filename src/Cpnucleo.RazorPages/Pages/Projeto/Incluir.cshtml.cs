namespace Cpnucleo.RazorPages.Pages.Projeto;

//[Authorize]
public class IncluirModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public IncluirModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public ProjetoDTO Projeto { get; set; }

    public SelectList SelectSistemas { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            IEnumerable<SistemaDTO> result = await _cpnucleoApiClient.GetAsync<IEnumerable<SistemaDTO>>("sistema", Token);
            SelectSistemas = new SelectList(result, "Id", "Nome");

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
                IEnumerable<SistemaDTO> result = await _cpnucleoApiClient.GetAsync<IEnumerable<SistemaDTO>>("sistema", Token);
                SelectSistemas = new SelectList(result, "Id", "Nome");

                return Page();
            }

            await _cpnucleoApiClient.PostAsync<ProjetoDTO>("projeto", Token, Projeto);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
