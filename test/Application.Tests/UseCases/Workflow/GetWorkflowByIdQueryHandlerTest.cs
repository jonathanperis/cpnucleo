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
        var workflow = Domain.Entities.Workflow.Create("Test Workflow", 1, BaseEntity.GetNewId());

        _workflowRepositoryMock
            .Setup(repo => repo.GetWorkflowById(It.IsAny<Guid>()))
            .ReturnsAsync(workflow);

        var query = new GetWorkflowByIdQuery(BaseEntity.GetNewId());

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
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new GetWorkflowByIdQuery(BaseEntity.GetNewId());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Workflow);
        _workflowRepositoryMock.Verify(repo => repo.GetWorkflowById(It.IsAny<Guid>()), Times.Once);
    }
}
