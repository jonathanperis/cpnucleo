using Cpnucleo.Domain.Entities;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces.Services
{
    public interface IRecursoTarefaService : ICrudService<RecursoTarefa>
    {
        IQueryable<RecursoTarefa> ListarPorTarefa(Guid idTarefa);
    }
}
