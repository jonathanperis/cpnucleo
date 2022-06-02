using Refit;
using AuthorizeAttribute = Refit.AuthorizeAttribute;

namespace Cpnucleo.RazorPages.Services.Interfaces;

public interface ICpnucleoApiClient
{
    [Get("/{route}/{action}")]
    Task<T> ExecuteQueryAsync<T>(string route, string action, [Authorize("Bearer")] string token, [Query] object value);

    [Post("/{route}/{action}")]
    Task<T> ExecuteCommandAsync<T>(string route, string action, [Authorize("Bearer")] string token, [Body] object value);
}
