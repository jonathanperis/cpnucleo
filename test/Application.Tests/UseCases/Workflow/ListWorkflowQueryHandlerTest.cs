namespace Application.Tests.UseCases.Workflow;

public class ListWorkflowQueryHandlerTest
{
    private readonly Mock<IWorkflowRepository> _workflowRepositoryMock;
    private readonly ListWorkflowQueryHandler _handler;

    public ListWorkflowQueryHandlerTest()
    {
        _workflowRepositoryMock = new Mock<IWorkflowRepository>();
        _handler = new ListWorkflowQueryHandler(_workflowRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfWorkflows_WhenWorkflowsExist()
    {
        // Arrange
        var workflows = new List<Domain.Entities.Workflow>
        {
            Domain.Entities.Workflow.Create("Test Workflow 1", 1, Guid.NewGuid()),
            Domain.Entities.Workflow.Create("Test Workflow 2", 2, Guid.NewGuid())
        };

        _workflowRepositoryMock
            .Setup(repo => repo.ListWorkflow())
            .ReturnsAsync(workflows);

        var query = new ListWorkflowQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Workflows);
        Assert.Equal(workflows.Count, result.Workflows.Count);
        _workflowRepositoryMock.Verify(repo => repo.ListWorkflow(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoWorkflowsExist()
    {
        // Arrange
        _workflowRepositoryMock
            .Setup(repo => repo.ListWorkflow())
            .ReturnsAsync([]);

        var query = new ListWorkflowQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Workflows);
        Assert.Empty(result.Workflows);
        _workflowRepositoryMock.Verify(repo => repo.ListWorkflow(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListWorkflowReturnsNull()
    {
        // Arrange
        _workflowRepositoryMock
            .Setup(repo => repo.ListWorkflow())
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new ListWorkflowQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Empty(result.Workflows);
        _workflowRepositoryMock.Verify(repo => repo.ListWorkflow(), Times.Once);
    }
}
