using Cpnucleo.Domain.Entities;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces.Services
{
    public interface IApontamentoService : ICrudService<Apontamento>
    {
        int ObterTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa);

        IQueryable<Apontamento> ListarPorRecurso(Guid idRecurso);
    }
}
