using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class RecursoService : BaseService<RecursoViewModel>, IRecursoService
    {
        private const string actionRoute = "recurso";

        public RecursoService(ISystemConfiguration systemConfiguration)
            : base(systemConfiguration)
        {
        }

        public async Task<bool> IncluirAsync(string token, RecursoViewModel obj)
        {
            return await PostAsync(token, actionRoute, obj);
        }

        public async Task<IEnumerable<RecursoViewModel>> ListarAsync(string token, bool getDependencies = false)
        {
            return await GetAsync(token, actionRoute, getDependencies);
        }

        public async Task<RecursoViewModel> ConsultarAsync(string token, Guid id)
        {
            return await GetAsync(token, actionRoute, id);
        }

        public async Task<bool> RemoverAsync(string token, Guid id)
        {
            return await DeleteAsync(token, actionRoute, id);
        }

        public async Task<bool> AlterarAsync(string token, RecursoViewModel obj)
        {
            return await PutAsync(token, actionRoute, obj.Id, obj);
        }

        public async Task<RecursoViewModel> AutenticarAsync(string login, string senha)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/autenticar", Method.GET);
                request.AddQueryParameter("login", login);
                request.AddQueryParameter("senha", senha);

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

                return JsonConvert.DeserializeObject<RecursoViewModel>(response.Content);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
