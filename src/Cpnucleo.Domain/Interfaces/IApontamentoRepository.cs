using Cpnucleo.Domain.Models;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IApontamentoRepository : IRepository<Apontamento>
    {
        void ApontarHoras(Apontamento apontamento);

        int ObterTotalHorasPoridRecurso(Guid idRecurso, Guid idTarefa);

        IEnumerable<Apontamento> ListarPoridRecurso(Guid idRecurso);
    }
}