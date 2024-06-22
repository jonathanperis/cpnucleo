namespace Application.Tests.UseCases.AssignmentImpediment;

public class UpdateAssignmentImpedimentCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly UpdateAssignmentImpedimentCommandHandler _handler;
    private readonly List<Domain.Entities.AssignmentImpediment> _assignmentImpediments;

    public UpdateAssignmentImpedimentCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _assignmentImpediments = new List<Domain.Entities.AssignmentImpediment>
        {
            Domain.Entities.AssignmentImpediment.Create("Test AssignmentImpediment 1", Ulid.NewUlid(), Ulid.NewUlid()),
            Domain.Entities.AssignmentImpediment.Create("Test AssignmentImpediment 2", Ulid.NewUlid(), Ulid.NewUlid())
        };

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
        var command = new UpdateAssignmentImpedimentCommand(Ulid.NewUlid(), "Updated AssignmentImpediment", Ulid.NewUlid(), Ulid.NewUlid());

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
        var command = new UpdateAssignmentImpedimentCommand(Ulid.Empty, "Test AssignmentImpediment", Ulid.NewUlid(), Ulid.NewUlid());
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
        var command = new UpdateAssignmentImpedimentCommand(Ulid.NewUlid(), string.Empty, Ulid.NewUlid(), Ulid.NewUlid());
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
        var command = new UpdateAssignmentImpedimentCommand(Ulid.NewUlid(), "Test AssignmentImpediment", Ulid.Empty, Ulid.NewUlid());
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
        var command = new UpdateAssignmentImpedimentCommand(Ulid.NewUlid(), "Test AssignmentImpediment", Ulid.NewUlid(), Ulid.Empty);
        var validator = new UpdateAssignmentImpedimentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "ImpedimentId"));
    }
}
