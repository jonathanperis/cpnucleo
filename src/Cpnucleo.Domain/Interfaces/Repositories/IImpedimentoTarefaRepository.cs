using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces.Repositories
{
    public interface IImpedimentoTarefaRepository : ICrudRepository<ImpedimentoTarefa>
    {
        IEnumerable<ImpedimentoTarefa> ListarPorTarefa(Guid idTarefa);
    }
}