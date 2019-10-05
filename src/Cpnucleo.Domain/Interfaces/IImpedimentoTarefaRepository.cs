using Cpnucleo.Domain.Models;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IImpedimentoTarefaRepository : ICrudRepository<ImpedimentoTarefa>
    {
        IEnumerable<ImpedimentoTarefa> ListarPorTarefa(Guid idTarefa);
    }
}