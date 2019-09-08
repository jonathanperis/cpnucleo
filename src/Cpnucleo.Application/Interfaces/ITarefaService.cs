using Cpnucleo.Application.ViewModels;
using System;

namespace Cpnucleo.Application.Interfaces
{
    public interface ITarefaAppService : IAppService<TarefaViewModel>
    {
        void AlterarPorFluxoTrabalho(Guid idTarefa, Guid idWorkflow);
    }
}
