using Cpnucleo.Domain.Models;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IImpedimentoTarefaRepository : ICrudRepository<ImpedimentoTarefa>
    {
        IQueryable<ImpedimentoTarefa> ConsultaPorTarefa(Guid idTarefa);

        IQueryable<ImpedimentoTarefa> ListarPorTarefa(Guid idTarefa);
    }
}