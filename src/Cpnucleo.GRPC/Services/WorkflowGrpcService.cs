namespace Cpnucleo.GRPC.Services;

[Authorize]
public class WorkflowGrpcService : ServiceBase<IWorkflowGrpcService>, IWorkflowGrpcService
{
    private readonly IMediator _mediator;

    public WorkflowGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UnaryResult<OperationResult> CreateWorkflow(CreateWorkflowCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<ListWorkflowViewModel> ListWorkflow(ListWorkflowQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<GetWorkflowViewModel> GetWorkflow(GetWorkflowQuery query)
    {
        return await _mediator.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveWorkflow(RemoveWorkflowCommand command)
    {
        return await _mediator.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateWorkflow(UpdateWorkflowCommand command)
    {
        return await _mediator.Send(command);
    }
}
