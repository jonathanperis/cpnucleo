using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class SistemaService : ICrudService<SistemaViewModel>
    {
        private readonly IHttpService _httpService;

        private const string actionRoute = "sistema";
        
        public SistemaService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<(IEnumerable<SistemaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarAsync(string token, bool getDependencies = false)
        {
            return await _httpService.GetAsync<IEnumerable<SistemaViewModel>>(actionRoute, token, getDependencies);
        }

        public async Task<(SistemaViewModel response, bool sucess, HttpStatusCode code, string message)> ConsultarAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<SistemaViewModel>(actionRoute, token, id);
        }

        public async Task<(SistemaViewModel response, bool sucess, HttpStatusCode code, string message)> IncluirAsync(string token, object value)
        {
            return await _httpService.PostAsync<SistemaViewModel>(actionRoute, token, value);
        }

        public async Task<(SistemaViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarAsync(string token, Guid id, object value)
        {
            return await _httpService.PutAsync<SistemaViewModel>(actionRoute, token, id, value);
        }

        public async Task<(SistemaViewModel response, bool sucess, HttpStatusCode code, string message)> RemoverAsync(string token, Guid id)
        {
            return await _httpService.DeleteAsync<SistemaViewModel>(actionRoute, token, id);
        }
    }
}