namespace Application.Tests.UseCases.UserAssignment;

public class UpdateUserAssignmentCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly UpdateUserAssignmentCommandHandler _handler;
    private readonly List<Domain.Entities.UserAssignment> _userAssignments;

    public UpdateUserAssignmentCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _userAssignments =
        [
            Domain.Entities.UserAssignment.Create(BaseEntity.GetNewId(), BaseEntity.GetNewId()),
            Domain.Entities.UserAssignment.Create(BaseEntity.GetNewId(), BaseEntity.GetNewId())
        ];

        _dbContextMock.Setup(db => db.UserAssignments).ReturnsDbSet(_userAssignments);

        _handler = new UpdateUserAssignmentCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserAssignmentIsUpdatedSuccessfully()
    {
        // Arrange
        var userAssignment = _userAssignments.First();
        var command = new UpdateUserAssignmentCommand(userAssignment.Id, BaseEntity.GetNewId(), BaseEntity.GetNewId());

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
        var userAssignment = _userAssignments.First();
        var command = new UpdateUserAssignmentCommand(userAssignment.Id, BaseEntity.GetNewId(), BaseEntity.GetNewId());

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
        var command = new UpdateUserAssignmentCommand(BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId());

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
        var command = new UpdateUserAssignmentCommand(Guid.Empty, BaseEntity.GetNewId(), BaseEntity.GetNewId());
        var validator = new UpdateUserAssignmentCommandValidator();

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
        var command = new UpdateUserAssignmentCommand(BaseEntity.GetNewId(), Guid.Empty, BaseEntity.GetNewId());
        var validator = new UpdateUserAssignmentCommandValidator();

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
        var command = new UpdateUserAssignmentCommand(BaseEntity.GetNewId(), BaseEntity.GetNewId(), Guid.Empty);
        var validator = new UpdateUserAssignmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "AssignmentId"));
    }
}
