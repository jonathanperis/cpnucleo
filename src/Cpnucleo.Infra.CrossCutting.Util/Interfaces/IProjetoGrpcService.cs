using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.CreateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.RemoveProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.UpdateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.GetProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.ListProjeto;
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
