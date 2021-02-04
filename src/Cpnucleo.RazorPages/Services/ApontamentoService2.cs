using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class ApontamentoService2 : IApontamentoService2
    {
        private readonly IHttpService _httpService;

        private const string actionRoute = "apontamento";
        
        public ApontamentoService2(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<(IEnumerable<ApontamentoViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarAsync(string token, bool getDependencies = false)
        {
            return await _httpService.GetAsync<IEnumerable<ApontamentoViewModel>>(actionRoute, token, getDependencies);
        }

        public async Task<(ApontamentoViewModel response, bool sucess, HttpStatusCode code, string message)> ConsultarAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<ApontamentoViewModel>(actionRoute, token, id);
        }

        public async Task<(ApontamentoViewModel response, bool sucess, HttpStatusCode code, string message)> IncluirAsync(string token, object value)
        {
            return await _httpService.PostAsync<ApontamentoViewModel>(actionRoute, token, value);
        }

        public async Task<(ApontamentoViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarAsync(string token, Guid id, object value)
        {
            return await _httpService.PutAsync<ApontamentoViewModel>(actionRoute, token, id, value);
        }

        public async Task<(ApontamentoViewModel response, bool sucess, HttpStatusCode code, string message)> RemoverAsync(string token, Guid id)
        {
            return await _httpService.DeleteAsync<ApontamentoViewModel>(actionRoute, token, id);
        }

        public async Task<(IEnumerable<ApontamentoViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarPorRecursoAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<IEnumerable<ApontamentoViewModel>>($"{actionRoute}/getbyrecurso", token, id);
        }
    }
}