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
    internal class ImpedimentoTarefaService : BaseService<ImpedimentoTarefaViewModel>, IImpedimentoTarefaService
    {
        private const string actionRoute = "impedimentoTarefa";

        public ImpedimentoTarefaService(ISystemConfiguration systemConfiguration)
            : base(systemConfiguration)
        {
        }

        public async Task<bool> IncluirAsync(string token, ImpedimentoTarefaViewModel obj)
        {
            return await PostAsync(token, actionRoute, obj);
        }

        public async Task<IEnumerable<ImpedimentoTarefaViewModel>> ListarAsync(string token, bool getDependencies = false)
        {
            return await GetAsync(token, actionRoute, getDependencies);
        }

        public async Task<ImpedimentoTarefaViewModel> ConsultarAsync(string token, Guid id)
        {
            return await GetAsync(token, actionRoute, id);
        }

        public async Task<bool> RemoverAsync(string token, Guid id)
        {
            return await DeleteAsync(token, actionRoute, id);
        }

        public async Task<bool> AlterarAsync(string token, ImpedimentoTarefaViewModel obj)
        {
            return await PutAsync(token, actionRoute, obj.Id, obj);
        }

        public async Task<IEnumerable<ImpedimentoTarefaViewModel>> ListarPorTarefaAsync(string token, Guid idTarefa)
        {
            try
            {
                RestRequest request = new RestRequest($"api/v2/{actionRoute}/getbytarefa/{idTarefa}", Method.GET);
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

                return JsonConvert.DeserializeObject<IEnumerable<ImpedimentoTarefaViewModel>>(response.Content);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
