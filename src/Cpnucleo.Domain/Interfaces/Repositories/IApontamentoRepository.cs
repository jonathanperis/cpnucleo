using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces.Repositories
{
    public interface IApontamentoRepository : ICrudRepository<Apontamento>
    {
        int ObterTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa);

        IEnumerable<Apontamento> ListarPorRecurso(Guid idRecurso);
    }
}