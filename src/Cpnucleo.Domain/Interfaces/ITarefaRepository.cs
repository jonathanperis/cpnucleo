using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Interfaces
{
    public interface ITarefaRepository : IGenericRepository<Tarefa>
    {
        Task<IEnumerable<Tarefa>> GetByRecursoAsync(Guid idRecurso);
    }
}