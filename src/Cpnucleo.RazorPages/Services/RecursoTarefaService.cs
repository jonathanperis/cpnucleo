using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class RecursoTarefaService : IRecursoTarefaService
    {
        private readonly IHttpService _httpService;

        private const string actionRoute = "recursoTarefa";
        
        public RecursoTarefaService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<(IEnumerable<RecursoTarefaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarAsync(string token, bool getDependencies = false)
        {
            return await _httpService.GetAsync<IEnumerable<RecursoTarefaViewModel>>(actionRoute, token, getDependencies);
        }

        public async Task<(RecursoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> ConsultarAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<RecursoTarefaViewModel>(actionRoute, token, id);
        }

        public async Task<(RecursoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> IncluirAsync(string token, object value)
        {
            return await _httpService.PostAsync<RecursoTarefaViewModel>(actionRoute, token, value);
        }

        public async Task<(RecursoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarAsync(string token, Guid id, object value)
        {
            return await _httpService.PutAsync<RecursoTarefaViewModel>(actionRoute, token, id, value);
        }

        public async Task<(RecursoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> RemoverAsync(string token, Guid id)
        {
            return await _httpService.DeleteAsync<RecursoTarefaViewModel>(actionRoute, token, id);
        }

        public async Task<(IEnumerable<RecursoTarefaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarPorTarefaAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<IEnumerable<RecursoTarefaViewModel>>($"{actionRoute}/getbytarefa", token, id);
        }
    }
}