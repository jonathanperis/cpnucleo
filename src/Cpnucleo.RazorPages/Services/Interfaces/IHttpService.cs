using System;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services.Interfaces
{
    public interface IHttpService
    {
        Task<(T response, bool sucess, HttpStatusCode code, string message)> GetAsync<T>(string uri, string token, bool getDependencies = false);

        Task<(T response, bool sucess, HttpStatusCode code, string message)> GetAsync<T>(string uri, string token, Guid id);

        Task<(T response, bool sucess, HttpStatusCode code, string message)> PostAsync<T>(string uri, string token, object value);

        Task<(T response, bool sucess, HttpStatusCode code, string message)> PutAsync<T>(string uri, string token, Guid id, object value);

        Task<(T response, bool sucess, HttpStatusCode code, string message)> DeleteAsync<T>(string uri, string token, Guid id);
    }
}