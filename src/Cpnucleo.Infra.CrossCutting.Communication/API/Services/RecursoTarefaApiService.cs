using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Services
{
    public class RecursoTarefaApiService : BaseApiService<RecursoTarefaViewModel>, IRecursoTarefaApiService
    {
        private const string actionRoute = "recursoTarefa";

        public async Task<bool> IncluirAsync(string token, RecursoTarefaViewModel obj)
        {
            return await PostAsync(token, actionRoute, obj);
        }

        public async Task<IEnumerable<RecursoTarefaViewModel>> ListarAsync(string token)
        {
            return await GetAsync(token, actionRoute);
        }

        public async Task<RecursoTarefaViewModel> ConsultarAsync(string token, Guid id)
        {
            return await GetAsync(token, actionRoute, id);
        }

        public async Task<bool> RemoverAsync(string token, Guid id)
        {
            return await DeleteAsync(token, actionRoute, id);
        }

        public async Task<bool> AlterarAsync(string token, RecursoTarefaViewModel obj)
        {
            return await PutAsync(token, actionRoute, obj.Id, obj);
        }

        public async Task<IEnumerable<RecursoTarefaViewModel>> ListarPorTarefaAsync(string token, Guid idTarefa)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/getbytarefa/{idTarefa}", Method.GET);
                request.AddHeader("Authorization", token);

                IRestResponse response = await _client.ExecuteAsync(request);

                return JsonConvert.DeserializeObject<IEnumerable<RecursoTarefaViewModel>>(response.Content);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
