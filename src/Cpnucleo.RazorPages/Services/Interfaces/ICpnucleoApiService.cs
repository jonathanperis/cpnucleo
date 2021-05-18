using Refit;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface ICpnucleoApiService
    {
        [Get("/{uri}")]
        Task<T> GetAsync<T>(string uri, [Authorize("Bearer")] string token, bool getDependencies = false);

        [Get("/{uri}/{id}")]
        Task<T> GetAsync<T>(string uri, [Authorize("Bearer")] string token, Guid id);

        [Post("/{uri}")]
        Task<T> PostAsync<T>(string uri, [Authorize("Bearer")] string token, [Body] object value);

        [Put("/{uri}/{id}")]
        Task PutAsync(string uri, [Authorize("Bearer")] string token, Guid id, [Body] object value);

        [Delete("/{uri}/{id}")]
        Task DeleteAsync(string uri, [Authorize("Bearer")] string token, Guid id);
    }
}