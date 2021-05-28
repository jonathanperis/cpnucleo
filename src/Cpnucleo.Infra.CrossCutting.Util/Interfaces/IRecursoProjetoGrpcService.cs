using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util
{
    public interface IRecursoProjetoGrpcService : IGenericGrpcService<RecursoProjetoViewModel>
    {
        Task<IEnumerable<RecursoProjetoViewModel>> GetByProjetoAsync(Guid idProjeto);
    }
}
