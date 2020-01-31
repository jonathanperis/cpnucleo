using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace Cpnucleo.Infra.CrossCutting.Communication.Services
{
    public class CrudApiService<TViewModel>
    {
        protected readonly RestClient _client;

        public CrudApiService()
        {
            _client = new RestClient("https://cpnucleo-api.azurewebsites.net/");
        }

        protected IEnumerable<TViewModel> Get(string token, string actionRoute)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}", Method.GET);
                request.AddHeader("Authorization", token);

                return JsonConvert.DeserializeObject<IEnumerable<TViewModel>>(_client.Execute(request).Content.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected TViewModel Get(string token, string actionRoute, Guid id)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/{id.ToString()}", Method.GET);
                request.AddHeader("Authorization", token);

                return JsonConvert.DeserializeObject<TViewModel>(_client.Execute(request).Content.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected bool Post(string token, string actionRoute, TViewModel obj)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}", Method.POST);
                request.AddHeader("Authorization", token);
                request.AddJsonBody(JsonConvert.SerializeObject(obj));

                var response = _client.Execute(request);

                return response.StatusCode == HttpStatusCode.Created ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected bool Put(string token, string actionRoute, Guid id, TViewModel obj)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/{id.ToString()}", Method.PUT);
                request.AddHeader("Authorization", token);
                request.AddJsonBody(JsonConvert.SerializeObject(obj));

                var response = _client.Execute(request);

                return response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected bool Delete(string token, string actionRoute, Guid id)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/{id.ToString()}", Method.DELETE);
                request.AddHeader("Authorization", token);

                var response = _client.Execute(request);

                return response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
