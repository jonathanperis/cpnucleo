namespace Cpnucleo.RazorPages.Pages.Projeto;

[Authorize]
public class AlterarModel : PageBase
{
    private readonly ICpnucleoApiService _cpnucleoApiService;

    public AlterarModel(ICpnucleoApiService cpnucleoApiService)
    {
        _cpnucleoApiService = cpnucleoApiService;
    }

    [BindProperty]
    public ProjetoViewModel Projeto { get; set; }

    public SelectList SelectSistemas { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            Projeto = await _cpnucleoApiService.GetAsync<ProjetoViewModel>("projeto", Token, id);

            IEnumerable<SistemaViewModel> result = await _cpnucleoApiService.GetAsync<IEnumerable<SistemaViewModel>>("sistema", Token);
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
                Projeto = await _cpnucleoApiService.GetAsync<ProjetoViewModel>("projeto", Token, Projeto.Id);

                IEnumerable<SistemaViewModel> result = await _cpnucleoApiService.GetAsync<IEnumerable<SistemaViewModel>>("sistema", Token);
                SelectSistemas = new SelectList(result, "Id", "Nome");

                return Page();
            }

            await _cpnucleoApiService.PutAsync("projeto", Token, Projeto.Id, Projeto);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
