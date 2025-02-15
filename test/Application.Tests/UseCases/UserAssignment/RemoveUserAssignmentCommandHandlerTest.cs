namespace Application.Tests.UseCases.UserAssignment;

public class RemoveUserAssignmentCommandHandlerTest
{
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly RemoveUserAssignmentCommandHandler _handler;
    private readonly List<Domain.Entities.UserAssignment> _userAssignments;

    public RemoveUserAssignmentCommandHandlerTest()
    {
        _dbContextMock = new Mock<ApplicationDbContext>();

        _userAssignments =
        [
            Domain.Entities.UserAssignment.Create(Ulid.NewUlid(), Ulid.NewUlid()),
            Domain.Entities.UserAssignment.Create(Ulid.NewUlid(), Ulid.NewUlid())
        ];

        _dbContextMock.Setup(db => db.UserAssignments).ReturnsDbSet(_userAssignments);

        _handler = new RemoveUserAssignmentCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserAssignmentIsRemovedSuccessfully()
    {
        // Arrange
        var userAssignmentId = _userAssignments.First().Id;
        var command = new RemoveUserAssignmentCommand(userAssignmentId);

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
        var userAssignmentId = _userAssignments.First().Id;
        var command = new RemoveUserAssignmentCommand(userAssignmentId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserAssignmentDoesNotExist()
    {
        // Arrange
        var userAssignmentId = Ulid.NewUlid();
        var command = new RemoveUserAssignmentCommand(userAssignmentId);

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
        var command = new RemoveUserAssignmentCommand(Ulid.Empty);
        var validator = new RemoveUserAssignmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
