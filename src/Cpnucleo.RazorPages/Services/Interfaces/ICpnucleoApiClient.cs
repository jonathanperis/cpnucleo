using Refit;

namespace Cpnucleo.RazorPages.Services.Interfaces;

[Headers("Authorization: Bearer")]
public interface ICpnucleoApiClient
{
    [Post("/{route}/{action}")]
    Task<T> ExecuteAsync<T>(string route, string action, [Body] object value);
}
