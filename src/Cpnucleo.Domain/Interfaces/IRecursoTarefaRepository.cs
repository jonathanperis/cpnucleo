using Cpnucleo.Domain.Models;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IRecursoTarefaRepository : IRepository<RecursoTarefa>
    {
        IQueryable<RecursoTarefa> ListarPoridTarefa(Guid idTarefa);

        IQueryable<RecursoTarefa> ListarPoridRecurso(Guid idRecurso);
    }
}