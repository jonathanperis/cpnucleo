using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces.Services
{
    public interface IApontamentoService : ICrudService<Apontamento>
    {
        int ObterTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa);

        IEnumerable<Apontamento> ListarPorRecurso(Guid idRecurso);
    }
}
