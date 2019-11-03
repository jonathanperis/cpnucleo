using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using RestSharp;
using Newtonsoft.Json;
using System;

namespace Cpnucleo.Infra.CrossCutting.Communication.Services
{
    public class RecursoApiService : CrudApiService<RecursoViewModel>, IRecursoApiService
    {
        public RecursoViewModel Autenticar(string login, string senha)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/recurso/autenticar", Method.GET);
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
