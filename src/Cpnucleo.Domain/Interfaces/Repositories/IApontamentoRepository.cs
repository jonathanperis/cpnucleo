using Cpnucleo.Domain.Entities;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces.Repositories
{
    public interface IApontamentoRepository : ICrudRepository<Apontamento>
    {
        int ObterTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa);

        IQueryable<Apontamento> ListarPorRecurso(Guid idRecurso);
    }
}