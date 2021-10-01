﻿namespace Cpnucleo.RazorPages.Pages.Impedimento;

[Authorize]
public class IncluirModel : PageBase
{
    private readonly ICpnucleoApiService _cpnucleoApiService;

    public IncluirModel(ICpnucleoApiService cpnucleoApiService)
    {
        _cpnucleoApiService = cpnucleoApiService;
    }

    [BindProperty]
    public ImpedimentoViewModel Impedimento { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _cpnucleoApiService.PostAsync<ImpedimentoViewModel>("impedimento", Token, Impedimento);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
