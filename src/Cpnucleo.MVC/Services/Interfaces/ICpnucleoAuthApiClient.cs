using Refit;
using AuthorizeAttribute = Refit.AuthorizeAttribute;

namespace Cpnucleo.MVC.Services.Interfaces;

public interface ICpnucleoAuthApiClient
{
    [Post("/{route}")]
    Task<T> PostAsync<T>(string route, [Authorize("Bearer")] string token, [Body] object value);
}
