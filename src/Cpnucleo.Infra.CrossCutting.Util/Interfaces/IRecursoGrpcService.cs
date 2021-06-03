using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.CreateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.RemoveRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.UpdateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.Auth;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.GetRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.ListRecurso;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface IRecursoGrpcService
    {
        [OperationContract]
        Task<CreateRecursoResponse> AddAsync(CreateRecursoCommand command, CallContext context = default);

        [OperationContract]
        Task<UpdateRecursoResponse> UpdateAsync(UpdateRecursoCommand command, CallContext context = default);

        [OperationContract]
        Task<GetRecursoResponse> GetAsync(GetRecursoQuery query, CallContext context = default);

        [OperationContract]
        Task<ListRecursoResponse> AllAsync(ListRecursoQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveRecursoResponse> RemoveAsync(RemoveRecursoCommand command, CallContext context = default);

        [OperationContract]
        Task<AuthResponse> AuthAsync(AuthQuery query, CallContext context = default);
    }
}
