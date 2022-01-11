using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Workflow.Services;

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

    public async UnaryResult<IEnumerable<WorkflowViewModel>> AllAsync(ListWorkflowQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<WorkflowViewModel> GetAsync(GetWorkflowQuery query)
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
