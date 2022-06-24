using Cpnucleo.Application.Commands.Workflow;
using Cpnucleo.Application.Queries.Workflow;
using Cpnucleo.Shared.Commands.Workflow;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.Workflow;

namespace Cpnucleo.Application.Test.Handlers;

public class WorkflowHandlerTest
{
    [Fact]
    public async Task CreateWorkflowCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        CreateWorkflowCommand request = MockCommandHelper.GetNewCreateWorkflowCommand();

        // Act
        CreateWorkflowHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetWorkflowQuery_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid workflowId = Guid.NewGuid();

        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));
        await unitOfWork.SaveChangesAsync();

        GetWorkflowQuery request = MockQueryHelper.GetNewGetWorkflowQuery(workflowId);

        // Act
        GetWorkflowHandler handler = new(unitOfWork, mapper);
        GetWorkflowViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Workflow != null);
        Assert.True(response.Workflow.Id != Guid.Empty);
        Assert.True(response.Workflow.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListWorkflowQuery_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        IWorkflowService service = WorkflowHelper.GetInstance();

        Guid workflowId = Guid.NewGuid();

        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow());
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow());

        await unitOfWork.SaveChangesAsync();

        ListWorkflowQuery request = MockQueryHelper.GetNewListWorkflowQuery();

        // Act
        ListWorkflowHandler handler = new(unitOfWork, mapper, service);
        ListWorkflowViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Workflows != null);
        Assert.True(response.Workflows.Any());
        Assert.True(response.Workflows.FirstOrDefault(x => x.Id == workflowId) != null);
        Assert.True(response.Workflows.FirstOrDefault(x => x.TamanhoColuna != null) != null);
    }

    [Fact]
    public async Task RemoveWorkflowCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid workflowId = Guid.NewGuid();

        Workflow workflow = MockEntityHelper.GetNewWorkflow(workflowId);

        await unitOfWork.WorkflowRepository.AddAsync(workflow);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.WorkflowRepository.Detatch(workflow);

        RemoveWorkflowCommand request = MockCommandHelper.GetNewRemoveWorkflowCommand(workflowId);
        GetWorkflowQuery request2 = MockQueryHelper.GetNewGetWorkflowQuery(workflowId);

        // Act
        RemoveWorkflowHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetWorkflowHandler handler2 = new(unitOfWork, mapper);
        GetWorkflowViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateWorkflowCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid workflowId = Guid.NewGuid();

        Workflow workflow = MockEntityHelper.GetNewWorkflow(workflowId);

        await unitOfWork.WorkflowRepository.AddAsync(workflow);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.WorkflowRepository.Detatch(workflow);

        UpdateWorkflowCommand request = MockCommandHelper.GetNewUpdateWorkflowCommand(workflowId);
        GetWorkflowQuery request2 = MockQueryHelper.GetNewGetWorkflowQuery(workflowId);

        // Act
        UpdateWorkflowHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetWorkflowHandler handler2 = new(unitOfWork, mapper);
        GetWorkflowViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Workflow != null);
        Assert.True(response2.Workflow.Id == workflowId);
    }
}
