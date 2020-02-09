using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;

namespace Cpnucleo.Application.Interfaces
{
    public interface ITarefaAppService : ICrudAppService<TarefaViewModel>
    {
        bool AlterarPorWorkflow(Guid idTarefa, Guid idWorkflow);
    }
}
