using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IRecursoProjetoRepository : IGenericRepository<RecursoProjetoViewModel>
    {
        Task<IEnumerable<RecursoProjetoViewModel>> GetByProjetoAsync(Guid idProjeto);
    }
}