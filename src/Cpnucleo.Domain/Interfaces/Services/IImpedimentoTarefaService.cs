using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces.Services
{
    public interface IImpedimentoTarefaService : ICrudService<ImpedimentoTarefa>
    {
        IEnumerable<ImpedimentoTarefa> ListarPorTarefa(Guid idTarefa);
    }
}
