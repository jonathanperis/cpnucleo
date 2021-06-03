using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.CreateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.RemoveSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.UpdateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.GetSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface ISistemaGrpcService
    {
        [OperationContract]
        Task<CreateSistemaResponse> AddAsync(CreateSistemaCommand command, CallContext context = default);

        [OperationContract]
        Task<UpdateSistemaResponse> UpdateAsync(UpdateSistemaCommand command, CallContext context = default);

        [OperationContract]
        Task<GetSistemaResponse> GetAsync(GetSistemaQuery query, CallContext context = default);

        [OperationContract]
        Task<ListSistemaResponse> AllAsync(ListSistemaQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveSistemaResponse> RemoveAsync(RemoveSistemaCommand command, CallContext context = default);
    }
}
