using Cpnucleo.Domain.Models;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces
{
    public interface ITarefaRepository : ICrudRepository<Tarefa>
    {
        IQueryable<Tarefa> ListarPorRecurso(Guid idRecurso);
    }
}