namespace Application.Tests.UseCases.UserAssignment;

public class CreateUserAssignmentCommandHandlerTest
{
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly CreateUserAssignmentCommandHandler _handler;

    public CreateUserAssignmentCommandHandlerTest()
    {
        _dbContextMock = new Mock<ApplicationDbContext>();

        Mock<DbSet<Domain.Entities.UserAssignment>> mockUserAssignmentsDbSet = new();
        _dbContextMock.Setup(db => db.UserAssignments).Returns(mockUserAssignmentsDbSet.Object);

        _handler = new CreateUserAssignmentCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserAssignmentIsCreatedSuccessfully()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new CreateUserAssignmentCommand(Ulid.NewUlid(), Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        _dbContextMock.Verify(db => db.UserAssignments!.AddAsync(It.IsAny<Domain.Entities.UserAssignment>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var command = new CreateUserAssignmentCommand(Ulid.NewUlid(), Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.UserAssignments!.AddAsync(It.IsAny<Domain.Entities.UserAssignment>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenUserIdIsEmpty()
    {
        // Arrange
        var command = new CreateUserAssignmentCommand(Ulid.Empty, Ulid.NewUlid());
        var validator = new CreateUserAssignmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "UserId"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenAssignmentIdIsEmpty()
    {
        // Arrange
        var command = new CreateUserAssignmentCommand(Ulid.NewUlid(), Ulid.Empty);
        var validator = new CreateUserAssignmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "AssignmentId"));
    }
}
