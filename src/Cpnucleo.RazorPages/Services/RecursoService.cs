using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class RecursoService : IRecursoService
    {
        private readonly IHttpService _httpService;

        private const string actionRoute = "recurso";
        
        public RecursoService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<(IEnumerable<RecursoViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarAsync(string token, bool getDependencies = false)
        {
            return await _httpService.GetAsync<IEnumerable<RecursoViewModel>>(actionRoute, token, getDependencies);
        }

        public async Task<(RecursoViewModel response, bool sucess, HttpStatusCode code, string message)> ConsultarAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<RecursoViewModel>(actionRoute, token, id);
        }

        public async Task<(RecursoViewModel response, bool sucess, HttpStatusCode code, string message)> IncluirAsync(string token, object value)
        {
            return await _httpService.PostAsync<RecursoViewModel>(actionRoute, token, value);
        }

        public async Task<(RecursoViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarAsync(string token, Guid id, object value)
        {
            return await _httpService.PutAsync<RecursoViewModel>(actionRoute, token, id, value);
        }

        public async Task<(RecursoViewModel response, bool sucess, HttpStatusCode code, string message)> RemoverAsync(string token, Guid id)
        {
            return await _httpService.DeleteAsync<RecursoViewModel>(actionRoute, token, id);
        }

        public async Task<(RecursoViewModel response, bool sucess, HttpStatusCode code, string message)> AutenticarAsync(string username, string password)
        {
            return await _httpService.PostAsync<RecursoViewModel>($"{actionRoute}/autenticar", "", new RecursoViewModel { Login = username, Senha = password });
        }
    }
}