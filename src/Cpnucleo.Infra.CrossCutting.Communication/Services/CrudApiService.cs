using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.Services
{
    public class CrudApiService<TViewModel> : ICrudApiService<TViewModel>
    {
        protected readonly RestClient _client;

        public CrudApiService()
        {
            _client = new RestClient("https://cpnucleo-api.azurewebsites.net/");
        }

        public IEnumerable<TViewModel> Get(string actionRoute)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}", Method.GET);

                return JsonConvert.DeserializeObject<IEnumerable<TViewModel>>(_client.Execute(request).Content.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TViewModel Get(string actionRoute, string parameter)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}", Method.GET);
                request.AddQueryParameter("id", parameter);

                return JsonConvert.DeserializeObject<TViewModel>(_client.Execute(request).Content.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Post(string actionRoute, TViewModel obj)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}", Method.POST);
                request.AddJsonBody(JsonConvert.SerializeObject(obj));

                _client.Execute(request);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Put(string actionRoute, string parameter, TViewModel obj)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}", Method.PUT);
                request.AddQueryParameter("id", parameter);
                request.AddJsonBody(JsonConvert.SerializeObject(obj));

                _client.Execute(request);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(string actionRoute, string parameter)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}", Method.DELETE);
                request.AddQueryParameter("id", parameter);

                _client.Execute(request);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
