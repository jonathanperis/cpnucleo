using Cpnucleo.Domain.Models;
using System;

namespace Cpnucleo.Domain.Interfaces
{
    public interface ITarefaRepository : IRepository<Tarefa>
    {
        void AlterarPorFluxoTrabalho(Guid idTarefa, Guid idWorkflow);
    }
}