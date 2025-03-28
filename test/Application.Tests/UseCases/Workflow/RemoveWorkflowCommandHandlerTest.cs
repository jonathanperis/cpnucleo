namespace Application.Tests.UseCases.Workflow;

public class RemoveWorkflowCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly RemoveWorkflowCommandHandler _handler;
    private readonly List<Domain.Entities.Workflow> _workflows;

    public RemoveWorkflowCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _workflows =
        [
            Domain.Entities.Workflow.Create("Test Workflow 1", 1, BaseEntity.GetNewId()),
            Domain.Entities.Workflow.Create("Test Workflow 2", 2, BaseEntity.GetNewId())
        ];

        _dbContextMock.Setup(db => db.Workflows).ReturnsDbSet(_workflows);

        _handler = new RemoveWorkflowCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenWorkflowIsRemovedSuccessfully()
    {
        // Arrange
        var workflowId = _workflows.First().Id;
        var command = new RemoveWorkflowCommand(workflowId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var workflowId = _workflows.First().Id;
        var command = new RemoveWorkflowCommand(workflowId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var command = new RemoveWorkflowCommand(Guid.Empty);
        var validator = new RemoveWorkflowCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
    
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenWorkflowDoesNotExist()
    {
        // Arrange
        var workflowId = BaseEntity.GetNewId();
        var command = new RemoveWorkflowCommand(workflowId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
