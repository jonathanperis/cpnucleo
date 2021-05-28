using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Sistema;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface ISistemaGrpcService
    {
        [OperationContract]
        Task<CreateSistemaResponse> AddAsync(CreateSistemaComand command, CallContext context = default);

        [OperationContract]
        Task<UpdateSistemaResponse> UpdateAsync(UpdateSistemaComand command, CallContext context = default);

        [OperationContract]
        Task<GetSistemaResponse> GetAsync(GetSistemaQuery query, CallContext context = default);

        [OperationContract]
        Task<ListSistemaResponse> AllAsync(ListSistemaQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveSistemaResponse> RemoveAsync(RemoveSistemaComand command, CallContext context = default);
    }
}
