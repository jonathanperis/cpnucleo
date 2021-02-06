using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class WorkflowService : ICrudService<WorkflowViewModel>
    {
        private readonly IHttpService _httpService;

        private const string actionRoute = "workflow";
        
        public WorkflowService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<(IEnumerable<WorkflowViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarAsync(string token, bool getDependencies = false)
        {
            return await _httpService.GetAsync<IEnumerable<WorkflowViewModel>>(actionRoute, token, getDependencies);
        }

        public async Task<(WorkflowViewModel response, bool sucess, HttpStatusCode code, string message)> ConsultarAsync(string token, Guid id)
        {
            return await _httpService.GetAsync<WorkflowViewModel>(actionRoute, token, id);
        }

        public async Task<(WorkflowViewModel response, bool sucess, HttpStatusCode code, string message)> IncluirAsync(string token, object value)
        {
            return await _httpService.PostAsync<WorkflowViewModel>(actionRoute, token, value);
        }

        public async Task<(WorkflowViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarAsync(string token, Guid id, object value)
        {
            return await _httpService.PutAsync<WorkflowViewModel>(actionRoute, token, id, value);
        }

        public async Task<(WorkflowViewModel response, bool sucess, HttpStatusCode code, string message)> RemoverAsync(string token, Guid id)
        {
            return await _httpService.DeleteAsync<WorkflowViewModel>(actionRoute, token, id);
        }
    }
}