using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces
{
    public interface ITarefaApiService : ICrudApiService<TarefaViewModel>
    {
        IEnumerable<TarefaViewModel> ListarPorRecurso(string token, Guid idRecurso);

        bool AlterarPorWorkflow(string token, Guid idTarefa, Guid idWorkflow);
    }
}
