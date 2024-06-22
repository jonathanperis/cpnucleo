namespace Application.Tests.UseCases.User;

public class CreateUserCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly CreateUserCommandHandler _handler;
    private readonly Mock<DbSet<Domain.Entities.User>> _mockUsersDbSet;

    public CreateUserCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _mockUsersDbSet = new Mock<DbSet<Domain.Entities.User>>();
        _dbContextMock.Setup(db => db.Users).Returns(_mockUsersDbSet.Object);

        _handler = new CreateUserCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserIsCreatedSuccessfully()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new CreateUserCommand("Test User", "testUser", "pass");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        _dbContextMock.Verify(db => db.Users!.AddAsync(It.IsAny<Domain.Entities.User>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var command = new CreateUserCommand("Test User", "testUser", "pass");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.Users!.AddAsync(It.IsAny<Domain.Entities.User>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateUserCommand(string.Empty, "testUser", "pass");
        var validator = new CreateUserCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Name"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenLoginIsEmpty()
    {
        // Arrange
        var command = new CreateUserCommand("Test User", string.Empty, "pass");
        var validator = new CreateUserCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Login"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenPasswordIsEmpty()
    {
        // Arrange
        var command = new CreateUserCommand("Test User", "testUser", string.Empty);
        var validator = new CreateUserCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Password"));
    }
}
