using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class BaseApiService<TViewModel>
    {
        protected readonly RestClient _client;

        public BaseApiService(ISystemConfiguration systemConfiguration)
        {
            _client = new RestClient(systemConfiguration.UrlCpnucleoApi);
        }

        protected async Task<IEnumerable<TViewModel>> GetAsync(string token, string actionRoute, bool getDependencies = false)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}", Method.GET);
                request.AddHeader("Authorization", $"Bearer {token}");
                request.AddQueryParameter("getDependencies", getDependencies.ToString());

                IRestResponse response = await _client.ExecuteAsync(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (!string.IsNullOrWhiteSpace(response.Content))
                    {
                        throw new Exception(response.Content);
                    }
                    else
                    {
                        throw new Exception("Falha ao se comunicar com a api de dados.");
                    }
                }

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
                request.AddHeader("Authorization", $"Bearer {token}");

                IRestResponse response = await _client.ExecuteAsync(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (!string.IsNullOrWhiteSpace(response.Content))
                    {
                        throw new Exception(response.Content);
                    }
                    else
                    {
                        throw new Exception("Falha ao se comunicar com a api de dados.");
                    }
                }

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
                request.AddHeader("Authorization", $"Bearer {token}");
                request.AddJsonBody(JsonConvert.SerializeObject(obj));

                IRestResponse response = await _client.ExecuteAsync(request);

                //if (response.StatusCode != HttpStatusCode.Created)
                //{
                //    if (!string.IsNullOrWhiteSpace(response.Content))
                //    {
                //        throw new Exception(response.Content);
                //    }
                //    else
                //    {
                //        throw new Exception("Falha ao se comunicar com a api de dados.");
                //    }
                //}

                //return response.StatusCode == HttpStatusCode.Created ? true : false;

                return true;
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
                request.AddHeader("Authorization", $"Bearer {token}");
                request.AddJsonBody(JsonConvert.SerializeObject(obj));

                IRestResponse response = await _client.ExecuteAsync(request);

                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    if (!string.IsNullOrWhiteSpace(response.Content))
                    {
                        throw new Exception(response.Content);
                    }
                    else
                    {
                        throw new Exception("Falha ao se comunicar com a api de dados.");
                    }
                }

                return response.StatusCode == HttpStatusCode.NoContent ? true : false;
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
                request.AddHeader("Authorization", $"Bearer {token}");

                IRestResponse response = await _client.ExecuteAsync(request);

                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    if (!string.IsNullOrWhiteSpace(response.Content))
                    {
                        throw new Exception(response.Content);
                    }
                    else
                    {
                        throw new Exception("Falha ao se comunicar com a api de dados.");
                    }
                }

                return response.StatusCode == HttpStatusCode.NoContent ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
