namespace Cpnucleo.RazorPages.Pages.Sistema;

//[Authorize]
public class RemoverModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public SistemaDTO Sistema { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            await CarregarSistema(id);

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
                await CarregarSistema(Sistema.Id);

                return Page();
            }

            var result = await _cpnucleoApiClient.ExecuteCommandAsync<OperationResult>("Sistema", "RemoveSistema", Token, new RemoveSistemaCommand { Id = Sistema.Id });

            if (result == OperationResult.Failed)
            {
                //@@JONATHAN - TRATAR ERRO.
            }

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    private async Task CarregarSistema(Guid id)
    {
        var result = await _cpnucleoApiClient.ExecuteQueryAsync<GetSistemaViewModel>("Sistema", "GetSistema", Token, new GetSistemaQuery { Id = id });

        if (result.OperationResult == OperationResult.Failed)
        {
            //@@JONATHAN - TRATAR ERRO.
        }

        Sistema = result.Sistema;
    }
}
