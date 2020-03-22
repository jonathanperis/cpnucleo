using Cpnucleo.Domain.Entities;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IRecursoTarefaRepository : ICrudRepository<RecursoTarefa>
    {
        IQueryable<RecursoTarefa> ListarPorTarefa(Guid idTarefa);
    }
}