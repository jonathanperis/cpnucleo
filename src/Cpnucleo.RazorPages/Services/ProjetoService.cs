using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class ProjetoService : ICrudService<ProjetoViewModel>
    {
        private readonly IHttpService _httpService;

        private const string actionRoute = "projeto";
        
        public ProjetoService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<(IEnumerable<ProjetoViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarAsync(string token, bool getDependencies = false)
        {
            return await _httpService.GetAsync<IEnumerable<ProjetoViewModel>>(actionRoute, token, getDependencies);
        }

        public async Task<(ProjetoViewModel response, bool sucess, HttpStatusCode code, string message)> ConsultarAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<ProjetoViewModel>(actionRoute, token, id);
        }

        public async Task<(ProjetoViewModel response, bool sucess, HttpStatusCode code, string message)> IncluirAsync(string token, object value)
        {
            return await _httpService.PostAsync<ProjetoViewModel>(actionRoute, token, value);
        }

        public async Task<(ProjetoViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarAsync(string token, Guid id, object value)
        {
            return await _httpService.PutAsync<ProjetoViewModel>(actionRoute, token, id, value);
        }

        public async Task<(ProjetoViewModel response, bool sucess, HttpStatusCode code, string message)> RemoverAsync(string token, Guid id)
        {
            return await _httpService.DeleteAsync<ProjetoViewModel>(actionRoute, token, id);
        }
    }
}