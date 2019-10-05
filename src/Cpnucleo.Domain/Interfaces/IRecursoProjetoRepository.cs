using Cpnucleo.Domain.Models;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IRecursoProjetoRepository : ICrudRepository<RecursoProjeto>
    {
        IEnumerable<RecursoProjeto> ListarPorProjeto(Guid idProjeto);
    }
}