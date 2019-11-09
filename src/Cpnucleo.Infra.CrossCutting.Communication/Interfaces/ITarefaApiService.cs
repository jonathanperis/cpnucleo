using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;

namespace Cpnucleo.Infra.CrossCutting.Communication.Interfaces
{
    public interface ITarefaApiService : ICrudApiService<TarefaViewModel>
    {
        bool AlterarPorPercentualConcluido(string token, Guid idTarefa, int? percentualConcluido);

        bool AlterarPorWorkflow(string token, Guid idTarefa, Guid idWorkflow);
    }
}
