using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Workflow;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface IWorkflowGrpcService
    {
        [OperationContract]
        Task<CreateWorkflowResponse> AddAsync(CreateWorkflowComand command, CallContext context = default);

        [OperationContract]
        Task<UpdateWorkflowResponse> UpdateAsync(UpdateWorkflowComand command, CallContext context = default);

        [OperationContract]
        Task<GetWorkflowResponse> GetAsync(GetWorkflowQuery query, CallContext context = default);

        [OperationContract]
        Task<ListWorkflowResponse> AllAsync(ListWorkflowQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveWorkflowResponse> RemoveAsync(RemoveWorkflowComand command, CallContext context = default);
    }
}
