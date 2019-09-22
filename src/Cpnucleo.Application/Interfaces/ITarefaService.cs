using Cpnucleo.Application.ViewModels;
using System;

namespace Cpnucleo.Application.Interfaces
{
    public interface ITarefaAppService : IAppService<TarefaViewModel>
    {
        bool AlterarPorPercentualConcluido(Guid idTarefa, int? percentualConcluido);

        bool AlterarPorWorkflow(Guid idTarefa, Guid idWorkflow);
    }
}
