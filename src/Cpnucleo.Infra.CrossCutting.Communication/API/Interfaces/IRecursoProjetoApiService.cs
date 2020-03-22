using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces
{
    public interface IRecursoProjetoApiService : ICrudApiService<RecursoProjetoViewModel>
    {
        Task<IEnumerable<RecursoProjetoViewModel>> ListarPorProjetoAsync(string token, Guid idProjeto);
    }
}
