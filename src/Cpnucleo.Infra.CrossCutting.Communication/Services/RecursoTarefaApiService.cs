using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.Services
{
    public class RecursoTarefaApiService : CrudApiService<RecursoTarefaViewModel>, IRecursoTarefaApiService
    {
        private const string actionRoute = "recursoTarefa";

        public bool Incluir(string token, RecursoTarefaViewModel obj)
        {
            return Post(token, actionRoute, obj);
        }

        public IEnumerable<RecursoTarefaViewModel> Listar(string token)
        {
            return Get(token, actionRoute);
        }

        public RecursoTarefaViewModel Consultar(string token, Guid id)
        {
            return Get(token, actionRoute, id);
        }

        public bool Remover(string token, Guid id)
        {
            return Delete(token, actionRoute, id);
        }

        public bool Alterar(string token, RecursoTarefaViewModel obj)
        {
            return Put(token, actionRoute, obj.Id, obj);
        }

        public IEnumerable<RecursoTarefaViewModel> ListarPorTarefa(string token, Guid idTarefa)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/getbytarefa/{idTarefa.ToString()}", Method.GET);
                request.AddHeader("Authorization", token);

                return JsonConvert.DeserializeObject<IEnumerable<RecursoTarefaViewModel>>(_client.Execute(request).Content.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<RecursoTarefaViewModel> ListarPorRecurso(string token, Guid idRecurso)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/getbyrecurso/{idRecurso.ToString()}", Method.GET);
                request.AddHeader("Authorization", token);

                return JsonConvert.DeserializeObject<IEnumerable<RecursoTarefaViewModel>>(_client.Execute(request).Content.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
