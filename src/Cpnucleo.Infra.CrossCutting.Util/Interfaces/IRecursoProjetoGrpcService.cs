using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.CreateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.RemoveRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.UpdateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetByProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.ListRecursoProjeto;
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
