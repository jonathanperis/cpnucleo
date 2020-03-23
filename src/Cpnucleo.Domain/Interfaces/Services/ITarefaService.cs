using Cpnucleo.Domain.Entities;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces.Services
{
    public interface ITarefaService : ICrudService<Tarefa>
    {
        IQueryable<Tarefa> ListarPorRecurso(Guid idRecurso);

        bool AlterarPorWorkflow(Guid idTarefa, Guid idWorkflow);
    }
}
