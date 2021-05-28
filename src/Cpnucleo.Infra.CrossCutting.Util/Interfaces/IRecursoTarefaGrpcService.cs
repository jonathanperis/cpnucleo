using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util
{
    public interface IRecursoTarefaGrpcService : IGenericGrpcService<RecursoTarefaViewModel>
    {
        Task<IEnumerable<RecursoTarefaViewModel>> GetByTarefaAsync(Guid idTarefa);
    }
}
