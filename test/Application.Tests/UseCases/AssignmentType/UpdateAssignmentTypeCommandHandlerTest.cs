namespace Application.Tests.UseCases.AssignmentType;

public class UpdateAssignmentTypeCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly UpdateAssignmentTypeCommandHandler _handler;
    private readonly List<Domain.Entities.AssignmentType> _assignmentTypes;

    public UpdateAssignmentTypeCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _assignmentTypes = new List<Domain.Entities.AssignmentType>
        {
            Domain.Entities.AssignmentType.Create("Test AssignmentType 1"),
            Domain.Entities.AssignmentType.Create("Test AssignmentType 2")
        };

        _dbContextMock.Setup(db => db.AssignmentTypes).ReturnsDbSet(_assignmentTypes);

        _handler = new UpdateAssignmentTypeCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAssignmentTypeIsUpdatedSuccessfully()
    {
        // Arrange
        var assignmentType = _assignmentTypes.First();
        var command = new UpdateAssignmentTypeCommand(assignmentType.Id, "Updated AssignmentType");

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        Assert.Equal("Updated AssignmentType", assignmentType.Name);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var assignmentType = _assignmentTypes.First();
        var command = new UpdateAssignmentTypeCommand(assignmentType.Id, "Updated AssignmentType");

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
        var command = new UpdateAssignmentTypeCommand(Ulid.NewUlid(), "Updated AssignmentType");

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
        var command = new UpdateAssignmentTypeCommand(Ulid.Empty, "Test AssignmentType");
        var validator = new UpdateAssignmentTypeCommandValidator();

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
        var command = new UpdateAssignmentTypeCommand(Ulid.NewUlid(), string.Empty);
        var validator = new UpdateAssignmentTypeCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Name"));
    }
}
