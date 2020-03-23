using Cpnucleo.Domain.Entities;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces.Services
{
    public interface IImpedimentoTarefaService : ICrudService<ImpedimentoTarefa>
    {
        IQueryable<ImpedimentoTarefa> ListarPorTarefa(Guid idTarefa);
    }
}
