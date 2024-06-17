namespace Application.Tests.UseCases.Project;

public class CreateProjectCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly CreateProjectCommandHandler _handler;
    private readonly Mock<DbSet<Domain.Entities.Project>> _mockProjectsDbSet;

    public CreateProjectCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _mockProjectsDbSet = new Mock<DbSet<Domain.Entities.Project>>();
        _dbContextMock.Setup(db => db.Projects).Returns(_mockProjectsDbSet.Object);

        _handler = new CreateProjectCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProjectIsCreatedSuccessfully()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new CreateProjectCommand("Test Project", Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        _dbContextMock.Verify(db => db.Projects!.AddAsync(It.IsAny<Domain.Entities.Project>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var command = new CreateProjectCommand("Test Project", Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.Projects!.AddAsync(It.IsAny<Domain.Entities.Project>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateProjectCommand(string.Empty, Ulid.NewUlid());
        var validator = new CreateProjectCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Name"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenSystemIdIsEmpty()
    {
        // Arrange
        var command = new CreateProjectCommand("Test Project", Ulid.Empty);
        var validator = new CreateProjectCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "SystemId"));
    }
}
