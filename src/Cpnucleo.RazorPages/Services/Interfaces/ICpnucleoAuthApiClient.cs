using Refit;
using AuthorizeAttribute = Refit.AuthorizeAttribute;

namespace Cpnucleo.RazorPages.Services.Interfaces;

public interface ICpnucleoAuthApiClient
{
    [Get("/{route}")]
    Task<T> GetAsync<T>(string route, [Authorize("Bearer")] string token, bool getDependencies = false);

    [Get("/{route}/{action}")]
    Task<T> GetAsync<T>(string route, string action, [Authorize("Bearer")] string token, bool getDependencies = false);

    [Get("/{route}/{id}")]
    Task<T> GetAsync<T>(string route, [Authorize("Bearer")] string token, Guid id);

    [Get("/{route}/{action}/{id}")]
    Task<T> GetAsync<T>(string route, string action, [Authorize("Bearer")] string token, Guid id);

    [Post("/{route}")]
    Task<T> PostAsync<T>(string route, [Authorize("Bearer")] string token, [Body] object value);

    [Post("/{route}/{action}")]
    Task<T> PostAsync<T>(string route, string action, [Authorize("Bearer")] string token, [Body] object value);

    [Put("/{route}/{id}")]
    Task PutAsync(string route, [Authorize("Bearer")] string token, Guid id, [Body] object value);

    [Put("/{route}/{action}/{id}")]
    Task PutAsync(string route, string action, [Authorize("Bearer")] string token, Guid id, [Body] object value);

    [Delete("/{route}/{id}")]
    Task DeleteAsync(string route, [Authorize("Bearer")] string token, Guid id);
}
