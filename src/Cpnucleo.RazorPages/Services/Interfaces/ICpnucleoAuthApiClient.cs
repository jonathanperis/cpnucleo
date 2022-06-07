using Refit;

namespace Cpnucleo.RazorPages.Services.Interfaces;

public interface ICpnucleoAuthApiClient
{
    [Post("/{route}")]
    Task<T> PostAsync<T>(string route, [Body] object value);
}
