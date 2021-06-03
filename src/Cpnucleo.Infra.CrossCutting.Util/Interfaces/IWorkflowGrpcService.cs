using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.CreateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.RemoveWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.UpdateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface IWorkflowGrpcService
    {
        [OperationContract]
        Task<CreateWorkflowResponse> AddAsync(CreateWorkflowCommand command, CallContext context = default);

        [OperationContract]
        Task<UpdateWorkflowResponse> UpdateAsync(UpdateWorkflowCommand command, CallContext context = default);

        [OperationContract]
        Task<GetWorkflowResponse> GetAsync(GetWorkflowQuery query, CallContext context = default);

        [OperationContract]
        Task<ListWorkflowResponse> AllAsync(ListWorkflowQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveWorkflowResponse> RemoveAsync(RemoveWorkflowCommand command, CallContext context = default);
    }
}
