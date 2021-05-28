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
        Task<CreateRecursoProjetoResponse> AddAsync(CreateRecursoProjetoComand command, CallContext context = default);

        [OperationContract]
        Task<UpdateRecursoProjetoResponse> UpdateAsync(UpdateRecursoProjetoComand command, CallContext context = default);

        [OperationContract]
        Task<GetRecursoProjetoResponse> GetAsync(GetRecursoProjetoQuery query, CallContext context = default);

        [OperationContract]
        Task<ListRecursoProjetoResponse> AllAsync(ListRecursoProjetoQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveRecursoProjetoResponse> RemoveAsync(RemoveRecursoProjetoComand command, CallContext context = default);

        [OperationContract]
        Task<GetByProjetoResponse> GetByProjetoAsync(GetByProjetoQuery query, CallContext context = default);
    }
}
