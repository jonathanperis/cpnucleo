using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces
{
    public interface ITarefaApiService : ICrudApiService<TarefaViewModel>
    {
        bool AlterarPorWorkflow(string token, Guid idTarefa, Guid idWorkflow);
    }
}
