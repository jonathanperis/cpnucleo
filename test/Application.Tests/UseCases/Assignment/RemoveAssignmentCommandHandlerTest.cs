namespace Application.Tests.UseCases.Assignment;

public class RemoveAssignmentCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly RemoveAssignmentCommandHandler _handler;
    private readonly List<Domain.Entities.Assignment> _assignments;

    public RemoveAssignmentCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _assignments = new List<Domain.Entities.Assignment>
        {
            Domain.Entities.Assignment.Create("Test Assignment 1", "Description 1", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 2, Ulid.NewUlid(), Ulid.NewUlid(), Ulid.NewUlid(), Ulid.NewUlid()),
            Domain.Entities.Assignment.Create("Test Assignment 2", "Description 2", DateTime.UtcNow, DateTime.UtcNow.AddDays(2), 3, Ulid.NewUlid(), Ulid.NewUlid(), Ulid.NewUlid(), Ulid.NewUlid())
        };

        _dbContextMock.Setup(db => db.Assignments).ReturnsDbSet(_assignments);

        _handler = new RemoveAssignmentCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAssignmentIsRemovedSuccessfully()
    {
        // Arrange
        var assignmentId = _assignments.First().Id;
        var command = new RemoveAssignmentCommand(assignmentId);

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
        var assignmentId = _assignments.First().Id;
        var command = new RemoveAssignmentCommand(assignmentId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenAssignmentDoesNotExist()
    {
        // Arrange
        var assignmentId = Ulid.NewUlid();
        var command = new RemoveAssignmentCommand(assignmentId);

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
        var command = new RemoveAssignmentCommand(Ulid.Empty);
        var validator = new RemoveAssignmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
