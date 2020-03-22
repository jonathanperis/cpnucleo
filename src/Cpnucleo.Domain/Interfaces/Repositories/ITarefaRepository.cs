using Cpnucleo.Domain.Entities;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces.Repositories
{
    public interface ITarefaRepository : ICrudRepository<Tarefa>
    {
        IQueryable<Tarefa> ListarPorRecurso(Guid idRecurso);
    }
}