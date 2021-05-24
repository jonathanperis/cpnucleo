using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Application.Interfaces
{
    public interface IRecursoTarefaAppService : IGenericAppService<RecursoTarefaViewModel>
    {
        Task<IEnumerable<RecursoTarefaViewModel>> GetByTarefaAsync(Guid idTarefa);
    }
}
