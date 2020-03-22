using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Services
{
    public class RecursoProjetoApiService : BaseApiService<RecursoProjetoViewModel>, IRecursoProjetoApiService
    {
        private const string actionRoute = "recursoProjeto";

        public async Task<bool> IncluirAsync(string token, RecursoProjetoViewModel obj)
        {
            return await PostAsync(token, actionRoute, obj);
        }

        public async Task<IEnumerable<RecursoProjetoViewModel>> ListarAsync(string token)
        {
            return await GetAsync(token, actionRoute);
        }

        public async Task<RecursoProjetoViewModel> ConsultarAsync(string token, Guid id)
        {
            return await GetAsync(token, actionRoute, id);
        }

        public async Task<bool> RemoverAsync(string token, Guid id)
        {
            return await DeleteAsync(token, actionRoute, id);
        }

        public async Task<bool> AlterarAsync(string token, RecursoProjetoViewModel obj)
        {
            return await PutAsync(token, actionRoute, obj.Id, obj);
        }

        public async Task<IEnumerable<RecursoProjetoViewModel>> ListarPorProjetoAsync(string token, Guid idProjeto)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/getbyprojeto/{idProjeto}", Method.GET);
                request.AddHeader("Authorization", token);

                IRestResponse response = await _client.ExecuteAsync(request);

                return JsonConvert.DeserializeObject<IEnumerable<RecursoProjetoViewModel>>(response.Content);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
