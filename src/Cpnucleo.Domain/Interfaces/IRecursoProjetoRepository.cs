using Cpnucleo.Domain.Models;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IRecursoProjetoRepository : ICrudRepository<RecursoProjeto>
    {
        IQueryable<RecursoProjeto> ListarPorProjeto(Guid idProjeto);
    }
}