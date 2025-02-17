namespace Application.Tests.UseCases.UserProject;

public class UpdateUserProjectCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly UpdateUserProjectCommandHandler _handler;
    private readonly List<Domain.Entities.UserProject> _userProjects;

    public UpdateUserProjectCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _userProjects =
        [
            Domain.Entities.UserProject.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
            Domain.Entities.UserProject.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
        ];

        _dbContextMock.Setup(db => db.UserProjects).ReturnsDbSet(_userProjects);

        _handler = new UpdateUserProjectCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserProjectIsUpdatedSuccessfully()
    {
        // Arrange
        var userProject = _userProjects.First();
        var command = new UpdateUserProjectCommand(userProject.Id, Guid.NewGuid(), Guid.NewGuid());

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        Assert.Equal(command.UserId, userProject.UserId);
        Assert.Equal(command.ProjectId, userProject.ProjectId);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var userProject = _userProjects.First();
        var command = new UpdateUserProjectCommand(userProject.Id, Guid.NewGuid(), Guid.NewGuid());

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
        var command = new UpdateUserProjectCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

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
        var command = new UpdateUserProjectCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid());
        var validator = new UpdateUserProjectCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenUserIdIsEmpty()
    {
        // Arrange
        var command = new UpdateUserProjectCommand(Guid.NewGuid(), Guid.Empty, Guid.NewGuid());
        var validator = new UpdateUserProjectCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "UserId"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenProjectIdIsEmpty()
    {
        // Arrange
        var command = new UpdateUserProjectCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty);
        var validator = new UpdateUserProjectCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "ProjectId"));
    }
}
