namespace Application.Tests.UseCases.Project;

public class RemoveProjectCommandHandlerTest
{
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly RemoveProjectCommandHandler _handler;
    private readonly List<Domain.Entities.Project> _projects;

    public RemoveProjectCommandHandlerTest()
    {
        _dbContextMock = new Mock<ApplicationDbContext>();

        _projects =
        [
            Domain.Entities.Project.Create("Test Project 1", Ulid.NewUlid(), Ulid.NewUlid()),
            Domain.Entities.Project.Create("Test Project 2", Ulid.NewUlid(), Ulid.NewUlid())
        ];

        _dbContextMock.Setup(db => db.Projects).ReturnsDbSet(_projects);

        _handler = new RemoveProjectCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProjectIsRemovedSuccessfully()
    {
        // Arrange
        var projectId = _projects.First().Id;
        var command = new RemoveProjectCommand(projectId);

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
        var projectId = _projects.First().Id;
        var command = new RemoveProjectCommand(projectId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        // Arrange
        var projectId = Ulid.NewUlid();
        var command = new RemoveProjectCommand(projectId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var command = new RemoveProjectCommand(Ulid.Empty);
        var validator = new RemoveProjectCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
