using Cpnucleo.Domain.Models;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IRecursoTarefaRepository : IRepository<RecursoTarefa>
    {
        IEnumerable<RecursoTarefa> ListarPoridTarefa(Guid idTarefa);

        IEnumerable<RecursoTarefa> ListarPoridRecurso(Guid idRecurso);
    }
}