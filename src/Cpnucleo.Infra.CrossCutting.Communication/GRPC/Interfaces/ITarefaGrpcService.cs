using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces
{
    public interface ITarefaGrpcService : ICrudGrpcService<TarefaViewModel>
    {
        Task<IEnumerable<TarefaViewModel>> ListarPorRecursoAsync(Guid idRecurso);

        Task<bool> AlterarPorWorkflowAsync(Guid idTarefa, Guid idWorkflow);
    }
}
