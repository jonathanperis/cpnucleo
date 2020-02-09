using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface ITarefaAppService : ICrudAppService<TarefaViewModel>
    {
        IEnumerable<TarefaViewModel> ListarPorRecurso(Guid idRecurso);

        bool AlterarPorWorkflow(Guid idTarefa, Guid idWorkflow);
    }
}
