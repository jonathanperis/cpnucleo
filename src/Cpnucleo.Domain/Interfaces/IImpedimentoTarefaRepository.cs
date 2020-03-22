using Cpnucleo.Domain.Entities;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IImpedimentoTarefaRepository : ICrudRepository<ImpedimentoTarefa>
    {
        IQueryable<ImpedimentoTarefa> ListarPorTarefa(Guid idTarefa);
    }
}