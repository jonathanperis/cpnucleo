using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Tarefa;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface ITarefaGrpcService
    {
        [OperationContract]
        Task<CreateTarefaResponse> AddAsync(CreateTarefaComand command, CallContext context = default);

        [OperationContract]
        Task<UpdateTarefaResponse> UpdateAsync(UpdateTarefaComand command, CallContext context = default);

        [OperationContract]
        Task<GetTarefaResponse> GetAsync(GetTarefaQuery query, CallContext context = default);

        [OperationContract]
        Task<ListTarefaResponse> AllAsync(ListTarefaQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveTarefaResponse> RemoveAsync(RemoveTarefaComand command, CallContext context = default);

        [OperationContract]
        Task<GetByRecursoResponse> GetByRecursoAsync(GetByRecursoQuery query);
    }
}
