using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.CreateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.RemoveWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.UpdateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class WorkflowGrpcService : ServiceBase<IWorkflowGrpcService>, IWorkflowGrpcService
{
    private readonly IAsyncRequestHandler<CreateWorkflowCommand, CreateWorkflowResponse> _createWorkflowCommand;
    private readonly IAsyncRequestHandler<ListWorkflowQuery, ListWorkflowResponse> _listWorkflowQuery;
    private readonly IAsyncRequestHandler<GetWorkflowQuery, GetWorkflowResponse> _getWorkflowQuery;
    private readonly IAsyncRequestHandler<RemoveWorkflowCommand, RemoveWorkflowResponse> _removeWorkflowCommand;
    private readonly IAsyncRequestHandler<UpdateWorkflowCommand, UpdateWorkflowResponse> _updateWorkflowCommand;

    public WorkflowGrpcService(IAsyncRequestHandler<CreateWorkflowCommand, CreateWorkflowResponse> createWorkflowCommand,
                               IAsyncRequestHandler<ListWorkflowQuery, ListWorkflowResponse> listWorkflowQuery,
                               IAsyncRequestHandler<GetWorkflowQuery, GetWorkflowResponse> getWorkflowQuery,
                               IAsyncRequestHandler<RemoveWorkflowCommand, RemoveWorkflowResponse> removeWorkflowCommand,
                               IAsyncRequestHandler<UpdateWorkflowCommand, UpdateWorkflowResponse> updateWorkflowCommand)
    {
        _createWorkflowCommand = createWorkflowCommand;
        _listWorkflowQuery = listWorkflowQuery;
        _getWorkflowQuery = getWorkflowQuery;
        _removeWorkflowCommand = removeWorkflowCommand;
        _updateWorkflowCommand = updateWorkflowCommand;
    }

    public async UnaryResult<CreateWorkflowResponse> AddAsync(CreateWorkflowCommand command)
    {
        return await _createWorkflowCommand.InvokeAsync(command);
    }

    public async UnaryResult<ListWorkflowResponse> AllAsync(ListWorkflowQuery query)
    {
        return await _listWorkflowQuery.InvokeAsync(query);
    }

    public async UnaryResult<GetWorkflowResponse> GetAsync(GetWorkflowQuery query)
    {
        return await _getWorkflowQuery.InvokeAsync(query);
    }

    public async UnaryResult<RemoveWorkflowResponse> RemoveAsync(RemoveWorkflowCommand command)
    {
        return await _removeWorkflowCommand.InvokeAsync(command);
    }

    public async UnaryResult<UpdateWorkflowResponse> UpdateAsync(UpdateWorkflowCommand command)
    {
        return await _updateWorkflowCommand.InvokeAsync(command);
    }
}
