using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.CreateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.RemoveImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.UpdateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.GetImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.ListImpedimento;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface IImpedimentoGrpcService
    {
        [OperationContract]
        Task<CreateImpedimentoResponse> AddAsync(CreateImpedimentoCommand command, CallContext context = default);

        [OperationContract]
        Task<UpdateImpedimentoResponse> UpdateAsync(UpdateImpedimentoCommand command, CallContext context = default);

        [OperationContract]
        Task<GetImpedimentoResponse> GetAsync(GetImpedimentoQuery query, CallContext context = default);

        [OperationContract]
        Task<ListImpedimentoResponse> AllAsync(ListImpedimentoQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveImpedimentoResponse> RemoveAsync(RemoveImpedimentoCommand command, CallContext context = default);
    }
}
