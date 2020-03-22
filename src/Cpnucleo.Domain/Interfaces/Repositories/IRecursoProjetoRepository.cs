using Cpnucleo.Domain.Entities;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces.Repositories
{
    public interface IRecursoProjetoRepository : ICrudRepository<RecursoProjeto>
    {
        IQueryable<RecursoProjeto> ListarPorProjeto(Guid idProjeto);
    }
}