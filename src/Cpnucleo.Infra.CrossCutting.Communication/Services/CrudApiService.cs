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

        public virtual IEnumerable<TViewModel> Get(string token, string actionRoute)
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

        public virtual TViewModel Get(string token, string actionRoute, Guid id)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}", Method.GET);
                request.AddHeader("Authorization", token);
                request.AddQueryParameter("id", id.ToString());

                return JsonConvert.DeserializeObject<TViewModel>(_client.Execute(request).Content.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual void Post(string token, string actionRoute, TViewModel obj)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}", Method.POST);
                request.AddHeader("Authorization", token);
                request.AddJsonBody(JsonConvert.SerializeObject(obj));

                _client.Execute(request);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual void Put(string token, string actionRoute, Guid id, TViewModel obj)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}", Method.PUT);
                request.AddHeader("Authorization", token);
                request.AddQueryParameter("id", id.ToString());
                request.AddJsonBody(JsonConvert.SerializeObject(obj));

                _client.Execute(request);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual void Delete(string token, string actionRoute, Guid id)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}", Method.DELETE);
                request.AddHeader("Authorization", token);
                request.AddQueryParameter("id", id.ToString());

                _client.Execute(request);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
