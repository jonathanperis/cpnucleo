namespace Cpnucleo.RazorPages.Pages.RecursoProjeto;

[Authorize]
public class IncluirModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public IncluirModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public RecursoProjetoDTO RecursoProjeto { get; set; }

    public ProjetoDTO Projeto { get; set; }

    public SelectList SelectRecursos { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid idProjeto)
    {
        try
        {
            Projeto = await _cpnucleoApiClient.GetAsync<ProjetoDTO>("projeto", Token, idProjeto);

            IEnumerable<RecursoDTO> result = await _cpnucleoApiClient.GetAsync<IEnumerable<RecursoDTO>>("recurso", Token);
            SelectRecursos = new SelectList(result, "Id", "Nome");

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
                Projeto = await _cpnucleoApiClient.GetAsync<ProjetoDTO>("projeto", Token, RecursoProjeto.IdProjeto);

                IEnumerable<RecursoDTO> result = await _cpnucleoApiClient.GetAsync<IEnumerable<RecursoDTO>>("recurso", Token);
                SelectRecursos = new SelectList(result, "Id", "Nome");

                return Page();
            }

            await _cpnucleoApiClient.PostAsync<RecursoProjetoDTO>("recursoProjeto", Token, RecursoProjeto);

            return RedirectToPage("Listar", new { idProjeto = RecursoProjeto.IdProjeto });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
