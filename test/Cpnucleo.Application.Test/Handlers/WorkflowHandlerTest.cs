using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Test.Handlers;

public class WorkflowHandlerTest
{
    [Fact]
    public async Task CreateWorkflowCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        CreateWorkflowCommand request = new()
        {
            Workflow = MockViewModelHelper.GetNewWorkflow()
        };

        // Act
        WorkflowHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetWorkflowQuery_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid workflowId = Guid.NewGuid();

        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));
        await unitOfWork.SaveChangesAsync();

        GetWorkflowQuery request = new()
        {
            Id = workflowId
        };

        // Act
        WorkflowHandler handler = new(unitOfWork, mapper);
        WorkflowViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Id != Guid.Empty);
        Assert.True(response.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListWorkflowQuery_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid workflowId = Guid.NewGuid();

        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow());
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow());

        await unitOfWork.SaveChangesAsync();

        ListWorkflowQuery request = new();

        // Act
        WorkflowHandler handler = new(unitOfWork, mapper);
        IEnumerable<WorkflowViewModel> response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Any());
        Assert.True(response.FirstOrDefault(x => x.Id == workflowId) != null);
        Assert.True(response.FirstOrDefault(x => x.TamanhoColuna != null) != null);
    }

    [Fact]
    public async Task RemoveWorkflowCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid workflowId = Guid.NewGuid();

        Workflow workflow = MockEntityHelper.GetNewWorkflow(workflowId);

        await unitOfWork.WorkflowRepository.AddAsync(workflow);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.WorkflowRepository.Detatch(workflow);

        RemoveWorkflowCommand request = new()
        {
            Id = workflowId
        };

        GetWorkflowQuery request2 = new()
        {
            Id = workflowId
        };

        // Act
        WorkflowHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        WorkflowViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 == null);
    }

    [Fact]
    public async Task UpdateWorkflowCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid workflowId = Guid.NewGuid();
        DateTime dataInclusao = DateTime.Now;

        Workflow workflow = MockEntityHelper.GetNewWorkflow(workflowId);

        await unitOfWork.WorkflowRepository.AddAsync(workflow);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.WorkflowRepository.Detatch(workflow);

        UpdateWorkflowCommand request = new()
        {
            Workflow = MockViewModelHelper.GetNewWorkflow(workflowId, dataInclusao)
        };

        GetWorkflowQuery request2 = new()
        {
            Id = workflowId
        };

        // Act
        WorkflowHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        WorkflowViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 != null);
        Assert.True(response2.Id == workflowId);
        Assert.True(response2.DataInclusao.Ticks == dataInclusao.Ticks);
    }
}
