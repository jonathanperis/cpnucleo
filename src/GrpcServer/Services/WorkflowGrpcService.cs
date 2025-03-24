namespace GrpcServer.Services;

internal class WorkflowGrpcService(ISender sender) : ServiceBase<IWorkflowGrpcService>, IWorkflowGrpcService
{
    public async UnaryResult<OperationResult> CreateWorkflow(CreateWorkflowCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<GetWorkflowByIdQueryViewModel> GetWorkflowById(GetWorkflowByIdQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<ListWorkflowQueryViewModel> ListWorkflow(ListWorkflowQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveWorkflow(RemoveWorkflowCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateWorkflow(UpdateWorkflowCommand command)
    {
        return await sender.Send(command);
    }
}