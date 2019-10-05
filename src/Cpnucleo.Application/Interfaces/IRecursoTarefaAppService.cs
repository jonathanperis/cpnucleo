using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface IRecursoTarefaAppService : ICrudAppService<RecursoTarefaViewModel>
    {
        IEnumerable<RecursoTarefaViewModel> ListarPorTarefa(Guid idTarefa);

        IEnumerable<RecursoTarefaViewModel> ListarPorRecurso(Guid idRecurso);
    }
}
