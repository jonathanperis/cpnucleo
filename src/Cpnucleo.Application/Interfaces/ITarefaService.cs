using Cpnucleo.Application.ViewModels;
using System;

namespace Cpnucleo.Application.Interfaces
{
    public interface ITarefaAppService : IAppService<TarefaViewModel>
    {
        bool AlterarPorApontamento(Guid idTarefa, int? percentualConcluido);

        void AlterarPorFluxoTrabalho(Guid idTarefa, Guid idWorkflow);
    }
}
