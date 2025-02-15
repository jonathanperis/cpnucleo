namespace Application.Tests.UseCases.User;

public class UpdateUserCommandHandlerTest
{
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly UpdateUserCommandHandler _handler;
    private readonly List<Domain.Entities.User> _users;

    public UpdateUserCommandHandlerTest()
    {
        _dbContextMock = new Mock<ApplicationDbContext>();

        _users =
        [
            Domain.Entities.User.Create("Test User 1", "testUser1", "password1"),
            Domain.Entities.User.Create("Test User 2", "testUser2", "password2")
        ];

        _dbContextMock.Setup(db => db.Users).ReturnsDbSet(_users);

        _handler = new UpdateUserCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserIsUpdatedSuccessfully()
    {
        // Arrange
        var user = _users.First();
        var command = new UpdateUserCommand(user.Id, "Updated User", "newPassword");

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        Assert.Equal("Updated User", user.Name);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var user = _users.First();
        var command = new UpdateUserCommand(user.Id, "Updated User", "newPassword");

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
        var command = new UpdateUserCommand(Ulid.NewUlid(), "Updated User", "newPassword");

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
        var command = new UpdateUserCommand(Ulid.Empty, "Test User", "password");
        var validator = new UpdateUserCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenNameIsEmpty()
    {
        // Arrange
        var command = new UpdateUserCommand(Ulid.NewUlid(), string.Empty, "password");
        var validator = new UpdateUserCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Name"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenPasswordIsEmpty()
    {
        // Arrange
        var command = new UpdateUserCommand(Ulid.NewUlid(), "Test User", string.Empty);
        var validator = new UpdateUserCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Password"));
    }
}
