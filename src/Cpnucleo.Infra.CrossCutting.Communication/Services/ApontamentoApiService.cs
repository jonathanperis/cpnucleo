using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.Services
{
    public class ApontamentoApiService : CrudApiService<ApontamentoViewModel>, IApontamentoApiService
    {
        private const string actionRoute = "apontamento";

        public bool Incluir(string token, ApontamentoViewModel obj)
        {
            return Post(token, actionRoute, obj);
        }

        public IEnumerable<ApontamentoViewModel> Listar(string token)
        {
            return Get(token, actionRoute);
        }

        public ApontamentoViewModel Consultar(string token, Guid id)
        {
            return Get(token, actionRoute, id);
        }

        public bool Remover(string token, Guid id)
        {
            return Delete(token, actionRoute, id);
        }

        public bool Alterar(string token, ApontamentoViewModel obj)
        {
            return Put(token, actionRoute, obj.Id, obj);
        }

        public IEnumerable<ApontamentoViewModel> ListarPorRecurso(string token, Guid id)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/getbyrecurso/{id.ToString()}", Method.GET);
                request.AddHeader("Authorization", token);

                return JsonConvert.DeserializeObject<IEnumerable<ApontamentoViewModel>>(_client.Execute(request).Content.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
