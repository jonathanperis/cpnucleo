using Cpnucleo.Domain.Models;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IRecursoProjetoRepository : IRepository<RecursoProjeto>
    {
        IEnumerable<RecursoProjeto> ListarPorProjeto(Guid idProjeto);
    }
}