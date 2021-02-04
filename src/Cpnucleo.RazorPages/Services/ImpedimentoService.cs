using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class ImpedimentoService : ICrudService<ImpedimentoViewModel>
    {
        private readonly IHttpService _httpService;

        private const string actionRoute = "impedimento";
        
        public ImpedimentoService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<(IEnumerable<ImpedimentoViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarAsync(string token, bool getDependencies = false)
        {
            return await _httpService.GetAsync<IEnumerable<ImpedimentoViewModel>>(actionRoute, token, getDependencies);
        }

        public async Task<(ImpedimentoViewModel response, bool sucess, HttpStatusCode code, string message)> ConsultarAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<ImpedimentoViewModel>(actionRoute, token, id);
        }

        public async Task<(ImpedimentoViewModel response, bool sucess, HttpStatusCode code, string message)> IncluirAsync(string token, object value)
        {
            return await _httpService.PostAsync<ImpedimentoViewModel>(actionRoute, token, value);
        }

        public async Task<(ImpedimentoViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarAsync(string token, Guid id, object value)
        {
            return await _httpService.PutAsync<ImpedimentoViewModel>(actionRoute, token, id, value);
        }

        public async Task<(ImpedimentoViewModel response, bool sucess, HttpStatusCode code, string message)> RemoverAsync(string token, Guid id)
        {
            return await _httpService.DeleteAsync<ImpedimentoViewModel>(actionRoute, token, id);
        }
    }
}