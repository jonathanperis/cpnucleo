using Cpnucleo.Domain.Models;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IImpedimentoTarefaRepository : IRepository<ImpedimentoTarefa>
    {
        IEnumerable<ImpedimentoTarefa> ListarPorTarefa(Guid idTarefa);
    }
}