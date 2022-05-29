namespace Cpnucleo.RazorPages.Pages.Projeto;

[Authorize]
public class AlterarModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public AlterarModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public ProjetoDTO Projeto { get; set; }

    public SelectList SelectSistemas { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            Projeto = await _cpnucleoApiClient.GetAsync<ProjetoDTO>("projeto", Token, id);

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
                Projeto = await _cpnucleoApiClient.GetAsync<ProjetoDTO>("projeto", Token, Projeto.Id);

                IEnumerable<SistemaDTO> result = await _cpnucleoApiClient.GetAsync<IEnumerable<SistemaDTO>>("sistema", Token);
                SelectSistemas = new SelectList(result, "Id", "Nome");

                return Page();
            }

            await _cpnucleoApiClient.PutAsync("projeto", Token, Projeto.Id, Projeto);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
