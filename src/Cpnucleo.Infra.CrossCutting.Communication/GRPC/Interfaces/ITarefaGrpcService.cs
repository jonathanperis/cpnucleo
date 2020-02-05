using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces
{
    public interface ITarefaGrpcService : ICrudGrpcService<TarefaViewModel>
    {
        Task<bool> AlterarPorPercentualConcluidoAsync(Guid idTarefa, int? percentualConcluido);

        Task<bool> AlterarPorWorkflowAsync(Guid idTarefa, Guid idWorkflow);
    }
}
