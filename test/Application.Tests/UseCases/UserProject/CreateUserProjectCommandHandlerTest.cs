namespace Application.Tests.UseCases.UserProject;

public class CreateUserProjectCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly CreateUserProjectCommandHandler _handler;

    public CreateUserProjectCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        Mock<DbSet<Domain.Entities.UserProject>> mockUserProjectsDbSet = new();
        _dbContextMock.Setup(db => db.UserProjects).Returns(mockUserProjectsDbSet.Object);

        _handler = new CreateUserProjectCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserProjectIsCreatedSuccessfully()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new CreateUserProjectCommand(Ulid.NewUlid(), Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        _dbContextMock.Verify(db => db.UserProjects!.AddAsync(It.IsAny<Domain.Entities.UserProject>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var command = new CreateUserProjectCommand(Ulid.NewUlid(), Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.UserProjects!.AddAsync(It.IsAny<Domain.Entities.UserProject>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenUserIdIsEmpty()
    {
        // Arrange
        var command = new CreateUserProjectCommand(Ulid.Empty, Ulid.NewUlid());
        var validator = new CreateUserProjectCommandValidator();

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
        var command = new CreateUserProjectCommand(Ulid.NewUlid(), Ulid.Empty);
        var validator = new CreateUserProjectCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "ProjectId"));
    }
}
