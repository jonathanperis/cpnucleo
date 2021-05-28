using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Workflow;
using MediatR;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    public class WorkflowGrpcService : IWorkflowGrpcService
    {
        private readonly IMediator _mediator;

        public WorkflowGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateWorkflowResponse> AddAsync(CreateWorkflowComand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<ListWorkflowResponse> AllAsync(ListWorkflowQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<GetWorkflowResponse> GetAsync(GetWorkflowQuery query, CallContext context = default)
        {
            return await _mediator.Send(query);
        }

        public async Task<RemoveWorkflowResponse> RemoveAsync(RemoveWorkflowComand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<UpdateWorkflowResponse> UpdateAsync(UpdateWorkflowComand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
