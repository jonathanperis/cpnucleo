using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.Services
{
    public class RecursoApiService : CrudApiService<RecursoViewModel>, IRecursoApiService
    {
        private const string actionRoute = "recurso";

        public bool Incluir(string token, RecursoViewModel obj)
        {
            return Post(token, actionRoute, obj);
        }

        public IEnumerable<RecursoViewModel> Listar(string token)
        {
            return Get(token, actionRoute);
        }

        public RecursoViewModel Consultar(string token, Guid id)
        {
            return Get(token, actionRoute, id);
        }

        public bool Remover(string token, Guid id)
        {
            return Delete(token, actionRoute, id);
        }

        public bool Alterar(string token, RecursoViewModel obj)
        {
            return Put(token, actionRoute, obj.Id, obj);
        }

        public RecursoViewModel Autenticar(string login, string senha)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/autenticar", Method.GET);
                request.AddQueryParameter("login", login);
                request.AddQueryParameter("senha", senha);

                return JsonConvert.DeserializeObject<RecursoViewModel>(_client.Execute(request).Content.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
