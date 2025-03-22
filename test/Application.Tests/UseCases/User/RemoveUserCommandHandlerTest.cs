namespace Application.Tests.UseCases.User;

public class RemoveUserCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly RemoveUserCommandHandler _handler;
    private readonly List<Domain.Entities.User> _users;

    public RemoveUserCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _users =
        [
            Domain.Entities.User.Create("Test User 1", "testUser1", "password1"),
            Domain.Entities.User.Create("Test User 2", "testUser2", "password2")
        ];

        _dbContextMock.Setup(db => db.Users).ReturnsDbSet(_users);

        _handler = new RemoveUserCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserIsRemovedSuccessfully()
    {
        // Arrange
        var userId = _users.First().Id;
        var command = new RemoveUserCommand(userId);

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
        var userId = _users.First().Id;
        var command = new RemoveUserCommand(userId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = BaseEntity.GetNewId();
        var command = new RemoveUserCommand(userId);

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
        var command = new RemoveUserCommand(Guid.Empty);
        var validator = new RemoveUserCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
