using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    public interface IApontamentoGrpcService : IGenericGrpcService<ApontamentoViewModel>
    {
        Task<IEnumerable<ApontamentoViewModel>> GetByRecursoAsync(Guid idRecurso);
        Task<int> GetTotalHorasPorRecursoAsync(Guid idRecurso, Guid id);
    }
}
