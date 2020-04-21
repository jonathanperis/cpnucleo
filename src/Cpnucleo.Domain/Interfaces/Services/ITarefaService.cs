using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces.Services
{
    public interface ITarefaService : ICrudService<Tarefa>
    {
        IEnumerable<Tarefa> ListarPorRecurso(Guid idRecurso);

        bool AlterarPorWorkflow(Guid idTarefa, Guid idWorkflow);
    }
}
