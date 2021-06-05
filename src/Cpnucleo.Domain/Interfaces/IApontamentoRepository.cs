using Cpnucleo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IApontamentoRepository : IGenericRepository<Apontamento>
    {
        Task<int> GetTotalHorasByRecursoAsync(Guid idRecurso, Guid idTarefa);

        Task<IEnumerable<Apontamento>> GetByRecursoAsync(Guid idRecurso);
    }
}