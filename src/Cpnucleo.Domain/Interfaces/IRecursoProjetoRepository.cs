using Cpnucleo.Domain.Models;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IRecursoProjetoRepository : IRepository<RecursoProjeto>
    {
        IQueryable<RecursoProjeto> ListarPoridProjeto(Guid idProjeto);
    }
}