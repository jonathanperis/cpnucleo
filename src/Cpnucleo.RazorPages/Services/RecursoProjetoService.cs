using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class RecursoProjetoService : IRecursoProjetoService
    {
        private readonly IHttpService _httpService;

        private const string actionRoute = "recursoProjeto";
        
        public RecursoProjetoService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<(IEnumerable<RecursoProjetoViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarAsync(string token, bool getDependencies = false)
        {
            return await _httpService.GetAsync<IEnumerable<RecursoProjetoViewModel>>(actionRoute, token, getDependencies);
        }

        public async Task<(RecursoProjetoViewModel response, bool sucess, HttpStatusCode code, string message)> ConsultarAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<RecursoProjetoViewModel>(actionRoute, token, id);
        }

        public async Task<(RecursoProjetoViewModel response, bool sucess, HttpStatusCode code, string message)> IncluirAsync(string token, object value)
        {
            return await _httpService.PostAsync<RecursoProjetoViewModel>(actionRoute, token, value);
        }

        public async Task<(RecursoProjetoViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarAsync(string token, Guid id, object value)
        {
            return await _httpService.PutAsync<RecursoProjetoViewModel>(actionRoute, token, id, value);
        }

        public async Task<(RecursoProjetoViewModel response, bool sucess, HttpStatusCode code, string message)> RemoverAsync(string token, Guid id)
        {
            return await _httpService.DeleteAsync<RecursoProjetoViewModel>(actionRoute, token, id);
        }

        public async Task<(IEnumerable<RecursoProjetoViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarPorProjetoAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<IEnumerable<RecursoProjetoViewModel>>($"{actionRoute}/getbyprojeto", token, id);
        }
    }
}