using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class WorkflowGrpcService : ServiceBase<IWorkflowGrpcService>, IWorkflowGrpcService
{
    private readonly IAsyncRequestHandler<CreateWorkflowCommand, OperationResult> _createWorkflowCommand;
    private readonly IAsyncRequestHandler<ListWorkflowQuery, IEnumerable<WorkflowViewModel>> _listWorkflowQuery;
    private readonly IAsyncRequestHandler<GetWorkflowQuery, WorkflowViewModel> _getWorkflowQuery;
    private readonly IAsyncRequestHandler<RemoveWorkflowCommand, OperationResult> _removeWorkflowCommand;
    private readonly IAsyncRequestHandler<UpdateWorkflowCommand, OperationResult> _updateWorkflowCommand;

    public WorkflowGrpcService(IAsyncRequestHandler<CreateWorkflowCommand, OperationResult> createWorkflowCommand,
                               IAsyncRequestHandler<ListWorkflowQuery, IEnumerable<WorkflowViewModel>> listWorkflowQuery,
                               IAsyncRequestHandler<GetWorkflowQuery, WorkflowViewModel> getWorkflowQuery,
                               IAsyncRequestHandler<RemoveWorkflowCommand, OperationResult> removeWorkflowCommand,
                               IAsyncRequestHandler<UpdateWorkflowCommand, OperationResult> updateWorkflowCommand)
    {
        _createWorkflowCommand = createWorkflowCommand;
        _listWorkflowQuery = listWorkflowQuery;
        _getWorkflowQuery = getWorkflowQuery;
        _removeWorkflowCommand = removeWorkflowCommand;
        _updateWorkflowCommand = updateWorkflowCommand;
    }

    public async UnaryResult<OperationResult> AddAsync(CreateWorkflowCommand command)
    {
        return await _createWorkflowCommand.InvokeAsync(command);
    }

    public async UnaryResult<IEnumerable<WorkflowViewModel>> AllAsync(ListWorkflowQuery query)
    {
        return await _listWorkflowQuery.InvokeAsync(query);
    }

    public async UnaryResult<WorkflowViewModel> GetAsync(GetWorkflowQuery query)
    {
        return await _getWorkflowQuery.InvokeAsync(query);
    }

    public async UnaryResult<OperationResult> RemoveAsync(RemoveWorkflowCommand command)
    {
        return await _removeWorkflowCommand.InvokeAsync(command);
    }

    public async UnaryResult<OperationResult> UpdateAsync(UpdateWorkflowCommand command)
    {
        return await _updateWorkflowCommand.InvokeAsync(command);
    }
}
