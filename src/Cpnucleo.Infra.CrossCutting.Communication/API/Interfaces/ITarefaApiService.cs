using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces
{
    public interface ITarefaApiService : ICrudApiService<TarefaViewModel>
    {
        Task<IEnumerable<TarefaViewModel>> ListarPorRecursoAsync(string token, Guid idRecurso);

        Task<bool> AlterarPorWorkflowAsync(string token, Guid idTarefa, Guid idWorkflow);
    }
}
