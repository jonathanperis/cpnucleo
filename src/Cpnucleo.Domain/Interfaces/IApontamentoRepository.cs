using Cpnucleo.Domain.Models;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IApontamentoRepository : ICrudRepository<Apontamento>
    {
        int ObterTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa);

        IEnumerable<Apontamento> ListarPorRecurso(Guid idRecurso);
    }
}