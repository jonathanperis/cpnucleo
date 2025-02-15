namespace Application.Tests.UseCases.Workflow;

public class UpdateWorkflowCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly UpdateWorkflowCommandHandler _handler;
    private readonly List<Domain.Entities.Workflow> _workflows;

    public UpdateWorkflowCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _workflows =
        [
            Domain.Entities.Workflow.Create("Test Workflow 1", 1, Ulid.NewUlid()),
            Domain.Entities.Workflow.Create("Test Workflow 2", 2, Ulid.NewUlid())
        ];

        _dbContextMock.Setup(db => db.Workflows).ReturnsDbSet(_workflows);

        _handler = new UpdateWorkflowCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenWorkflowIsUpdatedSuccessfully()
    {
        // Arrange
        var workflow = _workflows.First();
        var command = new UpdateWorkflowCommand(workflow.Id, "Updated Workflow", workflow.Order);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        Assert.Equal("Updated Workflow", workflow.Name);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var workflow = _workflows.First();
        var command = new UpdateWorkflowCommand(workflow.Id, "Updated Workflow", workflow.Order);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenWorkflowDoesNotExist()
    {
        // Arrange
        var command = new UpdateWorkflowCommand(Ulid.NewUlid(), "Updated Workflow", 1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
