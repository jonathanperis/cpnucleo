using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces
{
    public interface IApontamentoApiService : ICrudApiService<ApontamentoViewModel>
    {
        Task<IEnumerable<ApontamentoViewModel>> ListarPorRecursoAsync(string token, Guid id);
    }
}
