using Cpnucleo.Domain.Models;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IImpedimentoTarefaRepository : IRepository<ImpedimentoTarefa>
    {
        IQueryable<ImpedimentoTarefa> ListarPoridTarefa(Guid idTarefa);
    }
}