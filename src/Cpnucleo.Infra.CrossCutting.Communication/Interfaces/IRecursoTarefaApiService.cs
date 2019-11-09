using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.Interfaces
{
    public interface IRecursoTarefaApiService : ICrudApiService<RecursoTarefaViewModel>
    {
        IEnumerable<RecursoTarefaViewModel> ListarPorTarefa(string token, Guid idTarefa);

        IEnumerable<RecursoTarefaViewModel> ListarPorRecurso(string token, Guid idRecurso);
    }
}
