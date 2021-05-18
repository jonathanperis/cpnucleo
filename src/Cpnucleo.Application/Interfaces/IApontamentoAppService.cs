using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IApontamentoRepository : IGenericRepository<ApontamentoViewModel>
    {
        Task<int> GetTotalHorasPorRecursoAsync(Guid idRecurso, Guid idTarefa);

        Task<IEnumerable<ApontamentoViewModel>> GetByRecursoAsync(Guid idRecurso);
    }
}