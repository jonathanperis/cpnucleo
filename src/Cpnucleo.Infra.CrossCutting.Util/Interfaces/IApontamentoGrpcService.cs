using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Apontamento;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface IApontamentoGrpcService
    {
        [OperationContract]
        Task<CreateApontamentoResponse> AddAsync(CreateApontamentoCommand command, CallContext context = default);

        [OperationContract]
        Task<UpdateApontamentoResponse> UpdateAsync(UpdateApontamentoCommand command, CallContext context = default);

        [OperationContract]
        Task<GetApontamentoResponse> GetAsync(GetApontamentoQuery query, CallContext context = default);

        [OperationContract]
        Task<ListApontamentoResponse> AllAsync(ListApontamentoQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveApontamentoResponse> RemoveAsync(RemoveApontamentoCommand command, CallContext context = default);

        [OperationContract]
        Task<GetByRecursoResponse> GetByRecursoAsync(GetByRecursoQuery query, CallContext context = default);

        [OperationContract]
        Task<GetTotalHorasPorRecursoResponse> GetTotalHorasPorRecursoAsync(GetTotalHorasPorRecursoQuery query, CallContext context = default);
    }
}
