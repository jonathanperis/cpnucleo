using Cpnucleo.Domain.Models;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IApontamentoRepository : IRepository<Apontamento>
    {
        void ApontarHoras(Apontamento apontamento);

        int ObterTotalHorasPoridRecurso(Guid idRecurso, Guid idTarefa);

        IQueryable<Apontamento> ListarPoridRecurso(Guid idRecurso);
    }
}