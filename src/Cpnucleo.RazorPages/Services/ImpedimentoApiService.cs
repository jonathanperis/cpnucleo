using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class ImpedimentoApiService : BaseApiService<ImpedimentoViewModel>, ICrudApiService<ImpedimentoViewModel>
    {
        private const string actionRoute = "impedimento";

        public ImpedimentoApiService(ISystemConfiguration systemConfiguration)
            : base(systemConfiguration)
        {
        }

        public async Task<bool> IncluirAsync(string token, ImpedimentoViewModel obj)
        {
            return await PostAsync(token, actionRoute, obj);
        }

        public async Task<IEnumerable<ImpedimentoViewModel>> ListarAsync(string token, bool getDependencies = false)
        {
            return await GetAsync(token, actionRoute, getDependencies);
        }

        public async Task<ImpedimentoViewModel> ConsultarAsync(string token, Guid id)
        {
            return await GetAsync(token, actionRoute, id);
        }

        public async Task<bool> RemoverAsync(string token, Guid id)
        {
            return await DeleteAsync(token, actionRoute, id);
        }

        public async Task<bool> AlterarAsync(string token, ImpedimentoViewModel obj)
        {
            return await PutAsync(token, actionRoute, obj.Id, obj);
        }
    }
}
