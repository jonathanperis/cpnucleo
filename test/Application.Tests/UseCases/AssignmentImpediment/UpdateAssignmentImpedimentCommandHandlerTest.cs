namespace Application.Tests.UseCases.AssignmentImpediment;

public class UpdateAssignmentImpedimentCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly UpdateAssignmentImpedimentCommandHandler _handler;
    private readonly List<Domain.Entities.AssignmentImpediment> _assignmentImpediments;

    public UpdateAssignmentImpedimentCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _assignmentImpediments =
        [
            Domain.Entities.AssignmentImpediment.Create("Test AssignmentImpediment 1", BaseEntity.GetNewId(), BaseEntity.GetNewId()),
            Domain.Entities.AssignmentImpediment.Create("Test AssignmentImpediment 2", BaseEntity.GetNewId(), BaseEntity.GetNewId())
        ];

        _dbContextMock.Setup(db => db.AssignmentImpediments).ReturnsDbSet(_assignmentImpediments);

        _handler = new UpdateAssignmentImpedimentCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAssignmentImpedimentIsUpdatedSuccessfully()
    {
        // Arrange
        var assignmentImpediment = _assignmentImpediments.First();
        var command = new UpdateAssignmentImpedimentCommand(assignmentImpediment.Id, "Updated AssignmentImpediment", assignmentImpediment.AssignmentId, assignmentImpediment.ImpedimentId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        Assert.Equal("Updated AssignmentImpediment", assignmentImpediment.Description);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var assignmentImpediment = _assignmentImpediments.First();
        var command = new UpdateAssignmentImpedimentCommand(assignmentImpediment.Id, "Updated AssignmentImpediment", assignmentImpediment.AssignmentId, assignmentImpediment.ImpedimentId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenAssignmentImpedimentDoesNotExist()
    {
        // Arrange
        var command = new UpdateAssignmentImpedimentCommand(BaseEntity.GetNewId(), "Updated AssignmentImpediment", BaseEntity.GetNewId(), BaseEntity.GetNewId());

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
        var command = new UpdateAssignmentImpedimentCommand(Guid.Empty, "Test AssignmentImpediment", BaseEntity.GetNewId(), BaseEntity.GetNewId());
        var validator = new UpdateAssignmentImpedimentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenDescriptionIsEmpty()
    {
        // Arrange
        var command = new UpdateAssignmentImpedimentCommand(BaseEntity.GetNewId(), string.Empty, BaseEntity.GetNewId(), BaseEntity.GetNewId());
        var validator = new UpdateAssignmentImpedimentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Description"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenAssignmentIdIsEmpty()
    {
        // Arrange
        var command = new UpdateAssignmentImpedimentCommand(BaseEntity.GetNewId(), "Test AssignmentImpediment", Guid.Empty, BaseEntity.GetNewId());
        var validator = new UpdateAssignmentImpedimentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "AssignmentId"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenImpedimentIdIsEmpty()
    {
        // Arrange
        var command = new UpdateAssignmentImpedimentCommand(BaseEntity.GetNewId(), "Test AssignmentImpediment", BaseEntity.GetNewId(), Guid.Empty);
        var validator = new UpdateAssignmentImpedimentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "ImpedimentId"));
    }
}
