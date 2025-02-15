namespace Application.Tests.UseCases.AssignmentType;

public class RemoveAssignmentTypeCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly RemoveAssignmentTypeCommandHandler _handler;
    private readonly List<Domain.Entities.AssignmentType> _assignmentTypes;

    public RemoveAssignmentTypeCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _assignmentTypes =
        [
            Domain.Entities.AssignmentType.Create("Test AssignmentType 1"),
            Domain.Entities.AssignmentType.Create("Test AssignmentType 2")
        ];

        _dbContextMock.Setup(db => db.AssignmentTypes).ReturnsDbSet(_assignmentTypes);

        _handler = new RemoveAssignmentTypeCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAssignmentTypeIsRemovedSuccessfully()
    {
        // Arrange
        var assignmentTypeId = _assignmentTypes.First().Id;
        var command = new RemoveAssignmentTypeCommand(assignmentTypeId);

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
        var assignmentTypeId = _assignmentTypes.First().Id;
        var command = new RemoveAssignmentTypeCommand(assignmentTypeId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenAssignmentTypeDoesNotExist()
    {
        // Arrange
        var assignmentTypeId = Ulid.NewUlid();
        var command = new RemoveAssignmentTypeCommand(assignmentTypeId);

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
        var command = new RemoveAssignmentTypeCommand(Ulid.Empty);
        var validator = new RemoveAssignmentTypeCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
