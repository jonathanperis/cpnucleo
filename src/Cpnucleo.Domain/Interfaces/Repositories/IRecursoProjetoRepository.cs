using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces.Repositories
{
    public interface IRecursoProjetoRepository : ICrudRepository<RecursoProjeto>
    {
        IEnumerable<RecursoProjeto> ListarPorProjeto(Guid idProjeto);
    }
}