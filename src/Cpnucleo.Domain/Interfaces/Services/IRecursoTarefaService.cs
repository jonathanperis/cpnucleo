using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces.Services
{
    public interface IRecursoTarefaService : ICrudService<RecursoTarefa>
    {
        IEnumerable<RecursoTarefa> ListarPorTarefa(Guid idTarefa);
    }
}
