namespace Application.Tests.UseCases.Workflow;

public class GetWorkflowByIdQueryHandlerTest
{
    private readonly Mock<IWorkflowRepository> _workflowRepositoryMock;
    private readonly GetWorkflowByIdQueryHandler _handler;

    public GetWorkflowByIdQueryHandlerTest()
    {
        _workflowRepositoryMock = new Mock<IWorkflowRepository>();
        _handler = new GetWorkflowByIdQueryHandler(_workflowRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnWorkflow_WhenWorkflowExists()
    {
        // Arrange
        var workflowDto = new WorkflowDto("Test Workflow", 1)
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        _workflowRepositoryMock
            .Setup(repo => repo.GetWorkflowById(It.IsAny<Guid>()))
            .ReturnsAsync(workflowDto);

        var query = new GetWorkflowByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Workflow);
        _workflowRepositoryMock.Verify(repo => repo.GetWorkflowById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenWorkflowDoesNotExist()
    {
        // Arrange
        _workflowRepositoryMock
            .Setup(repo => repo.GetWorkflowById(It.IsAny<Guid>()))
            .ReturnsAsync((WorkflowDto?)null);

        var query = new GetWorkflowByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Workflow);
        _workflowRepositoryMock.Verify(repo => repo.GetWorkflowById(It.IsAny<Guid>()), Times.Once);
    }
}
