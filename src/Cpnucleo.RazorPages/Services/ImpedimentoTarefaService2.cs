using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class ImpedimentoTarefaService2 : IImpedimentoTarefaService2
    {
        private readonly IHttpService _httpService;

        private const string actionRoute = "impedimentoTarefa";
        
        public ImpedimentoTarefaService2(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<(IEnumerable<ImpedimentoTarefaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarAsync(string token, bool getDependencies = false)
        {
            return await _httpService.GetAsync<IEnumerable<ImpedimentoTarefaViewModel>>(actionRoute, token, getDependencies);
        }

        public async Task<(ImpedimentoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> ConsultarAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<ImpedimentoTarefaViewModel>(actionRoute, token, id);
        }

        public async Task<(ImpedimentoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> IncluirAsync(string token, object value)
        {
            return await _httpService.PostAsync<ImpedimentoTarefaViewModel>(actionRoute, token, value);
        }

        public async Task<(ImpedimentoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarAsync(string token, Guid id, object value)
        {
            return await _httpService.PutAsync<ImpedimentoTarefaViewModel>(actionRoute, token, id, value);
        }

        public async Task<(ImpedimentoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> RemoverAsync(string token, Guid id)
        {
            return await _httpService.DeleteAsync<ImpedimentoTarefaViewModel>(actionRoute, token, id);
        }

        public async Task<(IEnumerable<ImpedimentoTarefaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarPorTarefaAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<IEnumerable<ImpedimentoTarefaViewModel>>($"{actionRoute}/getbytarefa", token, id);
        }
    }
}