using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface IRecursoProjetoGrpcService
    {
        [OperationContract]
        Task<CreateRecursoProjetoResponse> AddAsync(CreateRecursoProjetoCommand command, CallContext context = default);

        [OperationContract]
        Task<UpdateRecursoProjetoResponse> UpdateAsync(UpdateRecursoProjetoCommand command, CallContext context = default);

        [OperationContract]
        Task<GetRecursoProjetoResponse> GetAsync(GetRecursoProjetoQuery query, CallContext context = default);

        [OperationContract]
        Task<ListRecursoProjetoResponse> AllAsync(ListRecursoProjetoQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveRecursoProjetoResponse> RemoveAsync(RemoveRecursoProjetoCommand command, CallContext context = default);

        [OperationContract]
        Task<GetByProjetoResponse> GetByProjetoAsync(GetByProjetoQuery query, CallContext context = default);
    }
}
