using Cpnucleo.Domain.Entities;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces.Repositories
{
    public interface IImpedimentoTarefaRepository : ICrudRepository<ImpedimentoTarefa>
    {
        IQueryable<ImpedimentoTarefa> ListarPorTarefa(Guid idTarefa);
    }
}