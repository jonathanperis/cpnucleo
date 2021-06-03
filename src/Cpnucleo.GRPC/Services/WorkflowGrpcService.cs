using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.CreateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.RemoveWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.UpdateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class WorkflowGrpcService : IWorkflowGrpcService
    {
        private readonly IMediator _mediator;

        public WorkflowGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateWorkflowResponse> AddAsync(CreateWorkflowCommand command, CallContext context = default)
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

        public async Task<RemoveWorkflowResponse> RemoveAsync(RemoveWorkflowCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }

        public async Task<UpdateWorkflowResponse> UpdateAsync(UpdateWorkflowCommand command, CallContext context = default)
        {
            return await _mediator.Send(command);
        }
    }
}
