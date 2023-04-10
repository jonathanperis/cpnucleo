namespace Cpnucleo.Application.Test.Handlers;

public class WorkflowHandlerTest
{
    [Fact]
    public async Task CreateWorkflowCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        await DbContextHelper.SeedData(context);

        CreateWorkflowCommand request = MockCommandHelper.GetNewCreateWorkflowCommand();

        // Act
        CreateWorkflowCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetWorkflowQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var workflow = context.Workflows.First();

        GetWorkflowQuery request = MockQueryHelper.GetNewGetWorkflowQuery(workflow.Id);

        // Act
        GetWorkflowQueryHandler handler = new(context, mapper);
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
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        ListWorkflowQuery request = MockQueryHelper.GetNewListWorkflowQuery();

        // Act
        ListWorkflowQueryHandler handler = new(context, mapper);
        ListWorkflowViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Workflows != null);
        Assert.True(response.Workflows.Any());
    }

    [Fact]
    public async Task RemoveWorkflowCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var workflow = context.Workflows.First();

        RemoveWorkflowCommand request = MockCommandHelper.GetNewRemoveWorkflowCommand(workflow.Id);
        GetWorkflowQuery request2 = MockQueryHelper.GetNewGetWorkflowQuery(workflow.Id);

        // Act
        RemoveWorkflowCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetWorkflowQueryHandler handler2 = new(context, mapper);
        GetWorkflowViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateWorkflowCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var workflow = context.Workflows.First();

        UpdateWorkflowCommand request = MockCommandHelper.GetNewUpdateWorkflowCommand(workflow.Id);
        GetWorkflowQuery request2 = MockQueryHelper.GetNewGetWorkflowQuery(workflow.Id);

        // Act
        UpdateWorkflowCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetWorkflowQueryHandler handler2 = new(context, mapper);
        GetWorkflowViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Workflow != null);
        Assert.True(response2.Workflow.Id == workflow.Id);
    }
}
