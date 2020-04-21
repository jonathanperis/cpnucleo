using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces.Repositories
{
    public interface ITarefaRepository : ICrudRepository<Tarefa>
    {
        IEnumerable<Tarefa> ListarPorRecurso(Guid idRecurso);
    }
}