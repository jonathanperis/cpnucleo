using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class WorkflowService : BaseService<WorkflowViewModel>, ICrudService<WorkflowViewModel>
    {
        private const string actionRoute = "workflow";

        public WorkflowService(ISystemConfiguration systemConfiguration)
            : base(systemConfiguration)
        {
        }

        public async Task<bool> IncluirAsync(string token, WorkflowViewModel obj)
        {
            return await PostAsync(token, actionRoute, obj);
        }

        public async Task<IEnumerable<WorkflowViewModel>> ListarAsync(string token, bool getDependencies = false)
        {
            return await GetAsync(token, actionRoute, getDependencies);
        }

        public async Task<WorkflowViewModel> ConsultarAsync(string token, Guid id)
        {
            return await GetAsync(token, actionRoute, id);
        }

        public async Task<bool> RemoverAsync(string token, Guid id)
        {
            return await DeleteAsync(token, actionRoute, id);
        }

        public async Task<bool> AlterarAsync(string token, WorkflowViewModel obj)
        {
            return await PutAsync(token, actionRoute, obj.Id, obj);
        }
    }
}
