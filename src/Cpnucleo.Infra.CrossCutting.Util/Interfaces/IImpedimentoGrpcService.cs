using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Impedimento;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface IImpedimentoGrpcService
    {
        [OperationContract]
        Task<CreateImpedimentoResponse> AddAsync(CreateImpedimentoComand command, CallContext context = default);

        [OperationContract]
        Task<UpdateImpedimentoResponse> UpdateAsync(UpdateImpedimentoComand command, CallContext context = default);

        [OperationContract]
        Task<GetImpedimentoResponse> GetAsync(GetImpedimentoQuery query, CallContext context = default);

        [OperationContract]
        Task<ListImpedimentoResponse> AllAsync(ListImpedimentoQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveImpedimentoResponse> RemoveAsync(RemoveImpedimentoComand command, CallContext context = default);
    }
}
