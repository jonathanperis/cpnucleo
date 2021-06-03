using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.CreateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.RemoveRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.UpdateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetByTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.ListRecursoTarefa;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface IRecursoTarefaGrpcService
    {
        [OperationContract]
        Task<CreateRecursoTarefaResponse> AddAsync(CreateRecursoTarefaCommand command, CallContext context = default);

        [OperationContract]
        Task<UpdateRecursoTarefaResponse> UpdateAsync(UpdateRecursoTarefaCommand command, CallContext context = default);

        [OperationContract]
        Task<GetRecursoTarefaResponse> GetAsync(GetRecursoTarefaQuery query, CallContext context = default);

        [OperationContract]
        Task<ListRecursoTarefaResponse> AllAsync(ListRecursoTarefaQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveRecursoTarefaResponse> RemoveAsync(RemoveRecursoTarefaCommand command, CallContext context = default);

        [OperationContract]
        Task<GetByTarefaResponse> GetByTarefaAsync(GetByTarefaQuery query);
    }
}
