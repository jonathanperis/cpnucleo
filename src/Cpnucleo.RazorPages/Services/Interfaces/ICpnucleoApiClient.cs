using Refit;
using AuthorizeAttribute = Refit.AuthorizeAttribute;

namespace Cpnucleo.RazorPages.Services.Interfaces;

[Headers("Authorization: Bearer")]
public interface ICpnucleoApiClient
{
    [Get("/{route}/{action}")]
    Task<T> ExecuteQueryAsync<T>(string route, string action, [Query] object value);

    [Post("/{route}/{action}")]
    Task<T> ExecuteCommandAsync<T>(string route, string action, [Body] object value);
}
