namespace Application.Tests.UseCases.UserProject;

public class RemoveUserProjectCommandHandlerTest
{
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly RemoveUserProjectCommandHandler _handler;
    private readonly List<Domain.Entities.UserProject> _userProjects;

    public RemoveUserProjectCommandHandlerTest()
    {
        _dbContextMock = new Mock<ApplicationDbContext>();

        _userProjects =
        [
            Domain.Entities.UserProject.Create(Ulid.NewUlid(), Ulid.NewUlid(), Ulid.NewUlid()),
            Domain.Entities.UserProject.Create(Ulid.NewUlid(), Ulid.NewUlid(), Ulid.NewUlid())
        ];

        _dbContextMock.Setup(db => db.UserProjects).ReturnsDbSet(_userProjects);

        _handler = new RemoveUserProjectCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserProjectIsRemovedSuccessfully()
    {
        // Arrange
        var userProjectId = _userProjects.First().Id;
        var command = new RemoveUserProjectCommand(userProjectId);

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
        var userProjectId = _userProjects.First().Id;
        var command = new RemoveUserProjectCommand(userProjectId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserProjectDoesNotExist()
    {
        // Arrange
        var userProjectId = Ulid.NewUlid();
        var command = new RemoveUserProjectCommand(userProjectId);

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
        var command = new RemoveUserProjectCommand(Ulid.Empty);
        var validator = new RemoveUserProjectCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
