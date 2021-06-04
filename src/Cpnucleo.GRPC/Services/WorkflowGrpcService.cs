using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.CreateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.RemoveWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.UpdateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;
using MagicOnion;
using MagicOnion.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC.Services
{
    [Authorize]
    public class WorkflowGrpcService : ServiceBase<IWorkflowGrpcService>, IWorkflowGrpcService
    {
        private readonly IMediator _mediator;

        public WorkflowGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async UnaryResult<CreateWorkflowResponse> AddAsync(CreateWorkflowCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<ListWorkflowResponse> AllAsync(ListWorkflowQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<GetWorkflowResponse> GetAsync(GetWorkflowQuery query)
        {
            return await _mediator.Send(query);
        }

        public async UnaryResult<RemoveWorkflowResponse> RemoveAsync(RemoveWorkflowCommand command)
        {
            return await _mediator.Send(command);
        }

        public async UnaryResult<UpdateWorkflowResponse> UpdateAsync(UpdateWorkflowCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
