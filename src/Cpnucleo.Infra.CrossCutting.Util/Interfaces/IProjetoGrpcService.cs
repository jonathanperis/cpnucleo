using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Projeto;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface IProjetoGrpcService
    {
        [OperationContract]
        Task<CreateProjetoResponse> AddAsync(CreateProjetoCommand command, CallContext context = default);

        [OperationContract]
        Task<UpdateProjetoResponse> UpdateAsync(UpdateProjetoCommand command, CallContext context = default);

        [OperationContract]
        Task<GetProjetoResponse> GetAsync(GetProjetoQuery query, CallContext context = default);

        [OperationContract]
        Task<ListProjetoResponse> AllAsync(ListProjetoQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveProjetoResponse> RemoveAsync(RemoveProjetoCommand command, CallContext context = default);
    }
}
