using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class TarefaService : ITarefaService
    {
        private readonly IHttpService _httpService;

        private const string actionRoute = "tarefa";
        
        public TarefaService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<(IEnumerable<TarefaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarAsync(string token, bool getDependencies = false)
        {
            return await _httpService.GetAsync<IEnumerable<TarefaViewModel>>(actionRoute, token, getDependencies);
        }

        public async Task<(TarefaViewModel response, bool sucess, HttpStatusCode code, string message)> ConsultarAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<TarefaViewModel>(actionRoute, token, id);
        }

        public async Task<(TarefaViewModel response, bool sucess, HttpStatusCode code, string message)> IncluirAsync(string token, object value)
        {
            return await _httpService.PostAsync<TarefaViewModel>(actionRoute, token, value);
        }

        public async Task<(TarefaViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarAsync(string token, Guid id, object value)
        {
            return await _httpService.PutAsync<TarefaViewModel>(actionRoute, token, id, value);
        }

        public async Task<(TarefaViewModel response, bool sucess, HttpStatusCode code, string message)> RemoverAsync(string token, Guid id)
        {
            return await _httpService.DeleteAsync<TarefaViewModel>(actionRoute, token, id);
        }

        public async Task<(IEnumerable<TarefaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarPorRecursoAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<IEnumerable<TarefaViewModel>>($"{actionRoute}/getbyrecurso", token, id);
        }

        public async Task<(TarefaViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarPorWorkflowAsync(string token, Guid id, object value)
        {
            return await _httpService.PutAsync<TarefaViewModel>($"{actionRoute}/putbyworkflow", token, id, value);
        }
    }
}