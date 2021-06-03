using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.CreateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.RemoveTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTarefa;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface ITarefaGrpcService
    {
        [OperationContract]
        Task<CreateTarefaResponse> AddAsync(CreateTarefaCommand command, CallContext context = default);

        [OperationContract]
        Task<UpdateTarefaResponse> UpdateAsync(UpdateTarefaCommand command, CallContext context = default);

        [OperationContract]
        Task<GetTarefaResponse> GetAsync(GetTarefaQuery query, CallContext context = default);

        [OperationContract]
        Task<ListTarefaResponse> AllAsync(ListTarefaQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveTarefaResponse> RemoveAsync(RemoveTarefaCommand command, CallContext context = default);

        [OperationContract]
        Task<GetByRecursoResponse> GetByRecursoAsync(GetByRecursoQuery query);
    }
}
