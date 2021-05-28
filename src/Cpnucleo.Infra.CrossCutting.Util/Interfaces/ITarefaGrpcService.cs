using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util
{
    public interface ITarefaGrpcService : IGenericGrpcService<TarefaViewModel>
    {
        Task<IEnumerable<TarefaViewModel>> GetByRecursoAsync(Guid idRecurso);
    }
}
