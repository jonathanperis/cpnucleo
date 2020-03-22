using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Services
{
    public class BaseApiService<TViewModel>
    {
        protected readonly RestClient _client;

        public BaseApiService()
        {
            //_client = new RestClient("https://localhost:5001");
            _client = new RestClient("https://cpnucleo-api.azurewebsites.net");
        }

        protected async Task<IEnumerable<TViewModel>> GetAsync(string token, string actionRoute)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}", Method.GET);
                request.AddHeader("Authorization", token);

                IRestResponse response = await _client.ExecuteAsync(request);

                return JsonConvert.DeserializeObject<IEnumerable<TViewModel>>(response.Content);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task<TViewModel> GetAsync(string token, string actionRoute, Guid id)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/{id}", Method.GET);
                request.AddHeader("Authorization", token);

                IRestResponse response = await _client.ExecuteAsync(request);

                return JsonConvert.DeserializeObject<TViewModel>(response.Content);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task<bool> PostAsync(string token, string actionRoute, TViewModel obj)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}", Method.POST);
                request.AddHeader("Authorization", token);
                request.AddJsonBody(JsonConvert.SerializeObject(obj));

                IRestResponse response = await _client.ExecuteAsync(request);

                return response.StatusCode == HttpStatusCode.Created ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task<bool> PutAsync(string token, string actionRoute, Guid id, TViewModel obj)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/{id}", Method.PUT);
                request.AddHeader("Authorization", token);
                request.AddJsonBody(JsonConvert.SerializeObject(obj));

                IRestResponse response = await _client.ExecuteAsync(request);

                return response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task<bool> DeleteAsync(string token, string actionRoute, Guid id)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/{id}", Method.DELETE);
                request.AddHeader("Authorization", token);

                IRestResponse response = await _client.ExecuteAsync(request);

                return response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
