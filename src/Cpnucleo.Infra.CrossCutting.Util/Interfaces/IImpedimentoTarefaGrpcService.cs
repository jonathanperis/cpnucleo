using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.ImpedimentoTarefa;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface IImpedimentoTarefaGrpcService
    {
        [OperationContract]
        Task<CreateImpedimentoTarefaResponse> AddAsync(CreateImpedimentoTarefaCommand command, CallContext context = default);

        [OperationContract]
        Task<UpdateImpedimentoTarefaResponse> UpdateAsync(UpdateImpedimentoTarefaCommand command, CallContext context = default);

        [OperationContract]
        Task<GetImpedimentoTarefaResponse> GetAsync(GetImpedimentoTarefaQuery query, CallContext context = default);

        [OperationContract]
        Task<ListImpedimentoTarefaResponse> AllAsync(ListImpedimentoTarefaQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveImpedimentoTarefaResponse> RemoveAsync(RemoveImpedimentoTarefaCommand command, CallContext context = default);

        [OperationContract]
        Task<GetByTarefaResponse> GetByTarefaAsync(GetByTarefaQuery query, CallContext context = default);
    }
}
