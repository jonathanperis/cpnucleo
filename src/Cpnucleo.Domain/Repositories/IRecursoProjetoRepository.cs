using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Repositories
{
    public interface IRecursoProjetoRepository : IGenericRepository<RecursoProjeto>
    {
        Task<IEnumerable<RecursoProjeto>> GetByProjetoAsync(Guid idProjeto);
    }
}