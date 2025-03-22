namespace Application.Tests.UseCases.Assignment;

public class UpdateAssignmentCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly UpdateAssignmentCommandHandler _handler;
    private readonly List<Domain.Entities.Assignment> _assignments;

    public UpdateAssignmentCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _assignments =
        [
            Domain.Entities.Assignment.Create("Test Assignment 1", "Description 1", DateTime.UtcNow,
                DateTime.UtcNow.AddDays(1), 2, BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId()),
            Domain.Entities.Assignment.Create("Test Assignment 2", "Description 2", DateTime.UtcNow,
                DateTime.UtcNow.AddDays(2), 3, BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId())
        ];

        _dbContextMock.Setup(db => db.Assignments).ReturnsDbSet(_assignments);

        _handler = new UpdateAssignmentCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAssignmentIsUpdatedSuccessfully()
    {
        // Arrange
        var assignment = _assignments.First();
        var command = new UpdateAssignmentCommand(assignment.Id, "Updated Assignment", "Updated Description", DateTime.UtcNow, DateTime.UtcNow.AddDays(2), 4, assignment.ProjectId, assignment.WorkflowId, assignment.UserId, assignment.AssignmentTypeId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        Assert.Equal("Updated Assignment", assignment.Name);
        Assert.Equal("Updated Description", assignment.Description);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var assignment = _assignments.First();
        var command = new UpdateAssignmentCommand(assignment.Id, "Updated Assignment", "Updated Description", DateTime.UtcNow, DateTime.UtcNow.AddDays(2), 4, assignment.ProjectId, assignment.WorkflowId, assignment.UserId, assignment.AssignmentTypeId);

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
        var command = new UpdateAssignmentCommand(BaseEntity.GetNewId(), "Updated Assignment", "Updated Description", DateTime.UtcNow, DateTime.UtcNow.AddDays(2), 4, BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId());

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
        var command = new UpdateAssignmentCommand(Guid.Empty, "Test Assignment", "Description", DateTime.UtcNow, DateTime.UtcNow.AddDays(2), 4, BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId());
        var validator = new UpdateAssignmentCommandValidator();

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
        var command = new UpdateAssignmentCommand(BaseEntity.GetNewId(), string.Empty, "Description", DateTime.UtcNow, DateTime.UtcNow.AddDays(2), 4, BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId());
        var validator = new UpdateAssignmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Name"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenDescriptionIsEmpty()
    {
        // Arrange
        var command = new UpdateAssignmentCommand(BaseEntity.GetNewId(), "Test Assignment", string.Empty, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), 4, BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId());
        var validator = new UpdateAssignmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Description"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenProjectIdIsEmpty()
    {
        // Arrange
        var command = new UpdateAssignmentCommand(BaseEntity.GetNewId(), "Test Assignment", "Description", DateTime.UtcNow, DateTime.UtcNow.AddDays(2), 4, Guid.Empty, BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId());
        var validator = new UpdateAssignmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "ProjectId"));
    }
}
