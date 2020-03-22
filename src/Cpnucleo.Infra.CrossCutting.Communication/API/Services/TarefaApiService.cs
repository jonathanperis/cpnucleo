using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Services
{
    public class TarefaApiService : BaseApiService<TarefaViewModel>, ITarefaApiService
    {
        private const string actionRoute = "tarefa";

        public async Task<bool> IncluirAsync(string token, TarefaViewModel obj)
        {
            return await PostAsync(token, actionRoute, obj);
        }

        public async Task<IEnumerable<TarefaViewModel>> ListarAsync(string token)
        {
            return await GetAsync(token, actionRoute);
        }

        public async Task<TarefaViewModel> ConsultarAsync(string token, Guid id)
        {
            return await GetAsync(token, actionRoute, id);
        }

        public async Task<bool> RemoverAsync(string token, Guid id)
        {
            return await DeleteAsync(token, actionRoute, id);
        }

        public async Task<bool> AlterarAsync(string token, TarefaViewModel obj)
        {
            return await PutAsync(token, actionRoute, obj.Id, obj);
        }

        public async Task<bool> AlterarPorWorkflowAsync(string token, Guid idTarefa, Guid idWorkflow)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/putbyworkflow/{idTarefa}", Method.PUT);
                request.AddHeader("Authorization", token);
                request.AddJsonBody(idWorkflow);

                IRestResponse response = await _client.ExecuteAsync(request);

                return response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TarefaViewModel>> ListarPorRecursoAsync(string token, Guid idRecurso)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/getbyrecurso/{idRecurso}", Method.GET);
                request.AddHeader("Authorization", token);

                IRestResponse response = await _client.ExecuteAsync(request);

                return JsonConvert.DeserializeObject<IEnumerable<TarefaViewModel>>(response.Content);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
