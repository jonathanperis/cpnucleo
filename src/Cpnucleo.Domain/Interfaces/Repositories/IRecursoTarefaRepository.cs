using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces.Repositories
{
    public interface IRecursoTarefaRepository : ICrudRepository<RecursoTarefa>
    {
        IEnumerable<RecursoTarefa> ListarPorTarefa(Guid idTarefa);
    }
}