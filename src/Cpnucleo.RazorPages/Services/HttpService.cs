using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.RazorPages.Services.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    public class HttpService : IHttpService
    {
        private readonly RestClient _client;

        public HttpService(ISystemConfiguration systemConfiguration)
        {
            _client = new RestClient($"{systemConfiguration.UrlCpnucleoApi}/api/v2/");
        }

        public async Task<(T response, bool sucess, HttpStatusCode code, string message)> GetAsync<T>(string uri, string token, bool getDependencies = false)
        {
            RestRequest request = new RestRequest(uri, Method.GET);
            request.AddQueryParameter("getDependencies", getDependencies.ToString());

            return await SendRequest<T>(request, token);
        }

        public async Task<(T response, bool sucess, HttpStatusCode code, string message)> GetAsync<T>(string uri, string token, Guid id)
        {
            RestRequest request = new RestRequest($"{uri}/{id}", Method.GET);

            return await SendRequest<T>(request, token);
        }        

        public async Task<(T response, bool sucess, HttpStatusCode code, string message)> PostAsync<T>(string uri, string token, object value)
        {
            RestRequest request = new RestRequest(uri, Method.POST);
            request.AddJsonBody(JsonConvert.SerializeObject(value));

            return await SendRequest<T>(request, token);
        }

        public async Task<(T response, bool sucess, HttpStatusCode code, string message)> PutAsync<T>(string uri, string token, Guid id, object value)
        {                
            RestRequest request = new RestRequest($"{uri}/{id}", Method.PUT);
            request.AddJsonBody(JsonConvert.SerializeObject(value));

            return await SendRequest<T>(request, token);
        }

        public async Task<(T response, bool sucess, HttpStatusCode code, string message)> DeleteAsync<T>(string uri, string token, Guid id)
        {
            RestRequest request = new RestRequest($"{uri}/{id}", Method.DELETE);

            return await SendRequest<T>(request, token);
        }                

        private async Task<(T response, bool sucess, HttpStatusCode code, string message)> SendRequest<T>(RestRequest request, string token)
        {
            request.AddHeader("Authorization", $"Bearer {token}");

            IRestResponse response = await _client.ExecuteAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return (default, false, response.StatusCode, string.Empty);

            if (!response.IsSuccessful)
                return (default, false, response.StatusCode, response.Content);

            if (response.StatusCode == HttpStatusCode.NoContent)
                return (default, true, response.StatusCode, string.Empty);

            return (JsonConvert.DeserializeObject<T>(response.Content), true, response.StatusCode, string.Empty);
        }
    }
}