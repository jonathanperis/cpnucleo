using Cpnucleo.Application.ViewModels;
using System;
using System.Linq;

namespace Cpnucleo.Application.Interfaces
{
    public interface IRecursoTarefaAppService : IAppService<RecursoTarefaViewModel>
    {
        IQueryable<RecursoTarefaViewModel> ListarPoridTarefa(Guid idTarefa);

        IQueryable<RecursoTarefaViewModel> ListarPoridRecurso(Guid idRecurso);
    }
}
