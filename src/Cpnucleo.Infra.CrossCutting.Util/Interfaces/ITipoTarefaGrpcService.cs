using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.CreateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.RemoveTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.UpdateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTipoTarefa;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface ITipoTarefaGrpcService
    {
        [OperationContract]
        Task<CreateTipoTarefaResponse> AddAsync(CreateTipoTarefaCommand command, CallContext context = default);

        [OperationContract]
        Task<UpdateTipoTarefaResponse> UpdateAsync(UpdateTipoTarefaCommand command, CallContext context = default);

        [OperationContract]
        Task<GetTipoTarefaResponse> GetAsync(GetTipoTarefaQuery query, CallContext context = default);

        [OperationContract]
        Task<ListTipoTarefaResponse> AllAsync(ListTipoTarefaQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveTipoTarefaResponse> RemoveAsync(RemoveTipoTarefaCommand command, CallContext context = default);
    }
}
