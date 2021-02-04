using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class TipoTarefaService : ICrudService<TipoTarefaViewModel>
    {
        private readonly IHttpService _httpService;

        private const string actionRoute = "tipoTarefa";
        
        public TipoTarefaService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<(IEnumerable<TipoTarefaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarAsync(string token, bool getDependencies = false)
        {
            return await _httpService.GetAsync<IEnumerable<TipoTarefaViewModel>>(actionRoute, token, getDependencies);
        }

        public async Task<(TipoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> ConsultarAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<TipoTarefaViewModel>(actionRoute, token, id);
        }

        public async Task<(TipoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> IncluirAsync(string token, object value)
        {
            return await _httpService.PostAsync<TipoTarefaViewModel>(actionRoute, token, value);
        }

        public async Task<(TipoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarAsync(string token, Guid id, object value)
        {
            return await _httpService.PutAsync<TipoTarefaViewModel>(actionRoute, token, id, value);
        }

        public async Task<(TipoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> RemoverAsync(string token, Guid id)
        {
            return await _httpService.DeleteAsync<TipoTarefaViewModel>(actionRoute, token, id);
        }
    }
}