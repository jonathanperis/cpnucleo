using Cpnucleo.RazorPages.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(T response, bool sucess, HttpStatusCode code, string message)> GetAsync<T>(string uri, string token, bool getDependencies = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{uri}?getDependencies={getDependencies}");

            return await SendRequest<T>(request, token);
        }

        public async Task<(T response, bool sucess, HttpStatusCode code, string message)> GetAsync<T>(string uri, string token, Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{uri}/{id}");

            return await SendRequest<T>(request, token);
        }        

        public async Task<(T response, bool sucess, HttpStatusCode code, string message)> PostAsync<T>(string uri, string token, object value)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");            

            return await SendRequest<T>(request, token);
        }

        public async Task<(T response, bool sucess, HttpStatusCode code, string message)> PutAsync<T>(string uri, string token, Guid id, object value)
        {                
            var request = new HttpRequestMessage(HttpMethod.Put, $"{uri}/{id}");
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");            

            return await SendRequest<T>(request, token);
        }

        public async Task<(T response, bool sucess, HttpStatusCode code, string message)> DeleteAsync<T>(string uri, string token, Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{uri}/{id}");

            return await SendRequest<T>(request, token);
        }                

        private async Task<(T response, bool sucess, HttpStatusCode code, string message)> SendRequest<T>(HttpRequestMessage request, string token)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return (default, false, response.StatusCode, string.Empty);

            if (!response.IsSuccessStatusCode)
                return (default, false, response.StatusCode, await response.Content.ReadAsStringAsync());

            if (response.StatusCode == HttpStatusCode.NoContent)
                return (default, true, response.StatusCode, string.Empty);

            return (await response.Content.ReadFromJsonAsync<T>(), true, response.StatusCode, string.Empty);
        }
    }
}