using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Application.Interfaces
{
    public interface IRecursoProjetoAppService : IGenericAppService<RecursoProjetoViewModel>
    {
        Task<IEnumerable<RecursoProjetoViewModel>> GetByProjetoAsync(Guid idProjeto);
    }
}
