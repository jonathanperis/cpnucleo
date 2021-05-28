using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoTarefa;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface IRecursoTarefaGrpcService
    {
        [OperationContract]
        Task<CreateRecursoTarefaResponse> AddAsync(CreateRecursoTarefaComand command, CallContext context = default);

        [OperationContract]
        Task<UpdateRecursoTarefaResponse> UpdateAsync(UpdateRecursoTarefaComand command, CallContext context = default);

        [OperationContract]
        Task<GetRecursoTarefaResponse> GetAsync(GetRecursoTarefaQuery query, CallContext context = default);

        [OperationContract]
        Task<ListRecursoTarefaResponse> AllAsync(ListRecursoTarefaQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveRecursoTarefaResponse> RemoveAsync(RemoveRecursoTarefaComand command, CallContext context = default);

        [OperationContract]
        Task<GetByTarefaResponse> GetByTarefaAsync(GetByTarefaQuery query);
    }
}
