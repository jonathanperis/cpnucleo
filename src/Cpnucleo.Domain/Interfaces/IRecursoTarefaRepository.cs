using Cpnucleo.Domain.Models;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IRecursoTarefaRepository : ICrudRepository<RecursoTarefa>
    {
        IEnumerable<RecursoTarefa> ListarPorTarefa(Guid idTarefa);

        IEnumerable<RecursoTarefa> ListarPorRecurso(Guid idRecurso);
    }
}