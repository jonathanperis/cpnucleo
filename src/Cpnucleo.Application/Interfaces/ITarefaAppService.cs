using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;

namespace Cpnucleo.Application.Interfaces
{
    public interface ITarefaAppService : ICrudAppService<TarefaViewModel>
    {
        bool AlterarPorPercentualConcluido(Guid idTarefa, int? percentualConcluido);

        bool AlterarPorWorkflow(Guid idTarefa, Guid idWorkflow);
    }
}
