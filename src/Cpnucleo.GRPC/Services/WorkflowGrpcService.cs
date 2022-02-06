using Cpnucleo.Application.Commands.Workflow.CreateWorkflow;
using Cpnucleo.Application.Commands.Workflow.RemoveWorkflow;
using Cpnucleo.Application.Commands.Workflow.UpdateWorkflow;
using Cpnucleo.Application.Queries.Workflow.GetWorkflow;
using Cpnucleo.Application.Queries.Workflow.ListWorkflow;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class WorkflowGrpcService : ServiceBase<IWorkflowGrpcService>, IWorkflowGrpcService
{
    private readonly IMediator _mediator;

    public WorkflowGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateWorkflowCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<ListWorkflowViewModel> AllAsync(ListWorkflowQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetWorkflowViewModel> GetAsync(GetWorkflowQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveWorkflowCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateWorkflowCommand command)
    {
        return await _mediator.Send(command);
    }
}
