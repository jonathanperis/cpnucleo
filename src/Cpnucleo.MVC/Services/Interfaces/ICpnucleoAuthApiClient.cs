using Refit;

namespace Cpnucleo.MVC.Services.Interfaces;

public interface ICpnucleoAuthApiClient
{
    [Post("/{route}")]
    Task<T> PostAsync<T>(string route, [Body] object value);
}
