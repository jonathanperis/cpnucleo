namespace Application.Tests.UseCases.Workflow;

public class CreateWorkflowCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly CreateWorkflowCommandHandler _handler;
    private readonly Mock<DbSet<Domain.Entities.Workflow>> _mockWorkflowsDbSet;

    public CreateWorkflowCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _mockWorkflowsDbSet = new Mock<DbSet<Domain.Entities.Workflow>>();
        _dbContextMock.Setup(db => db.Workflows).Returns(_mockWorkflowsDbSet.Object);

        _handler = new CreateWorkflowCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenWorkflowIsCreatedSuccessfully()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new CreateWorkflowCommand("Test Workflow", 1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        _dbContextMock.Verify(db => db.Workflows!.AddAsync(It.IsAny<Domain.Entities.Workflow>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var command = new CreateWorkflowCommand("Test Workflow", 1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.Workflows!.AddAsync(It.IsAny<Domain.Entities.Workflow>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
