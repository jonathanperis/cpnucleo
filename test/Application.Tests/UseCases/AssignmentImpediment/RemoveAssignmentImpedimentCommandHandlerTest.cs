namespace Application.Tests.UseCases.AssignmentImpediment;

public class RemoveAssignmentImpedimentCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly RemoveAssignmentImpedimentCommandHandler _handler;
    private readonly List<Domain.Entities.AssignmentImpediment> _assignmentImpediments;

    public RemoveAssignmentImpedimentCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _assignmentImpediments =
        [
            Domain.Entities.AssignmentImpediment.Create("Test AssignmentImpediment 1", Ulid.NewUlid(), Ulid.NewUlid()),
            Domain.Entities.AssignmentImpediment.Create("Test AssignmentImpediment 2", Ulid.NewUlid(), Ulid.NewUlid())
        ];

        _dbContextMock.Setup(db => db.AssignmentImpediments).ReturnsDbSet(_assignmentImpediments);

        _handler = new RemoveAssignmentImpedimentCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAssignmentImpedimentIsRemovedSuccessfully()
    {
        // Arrange
        var assignmentImpedimentId = _assignmentImpediments.First().Id;
        var command = new RemoveAssignmentImpedimentCommand(assignmentImpedimentId);

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
        var assignmentImpedimentId = _assignmentImpediments.First().Id;
        var command = new RemoveAssignmentImpedimentCommand(assignmentImpedimentId);

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
        var assignmentImpedimentId = Ulid.NewUlid();
        var command = new RemoveAssignmentImpedimentCommand(assignmentImpedimentId);

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
        var command = new RemoveAssignmentImpedimentCommand(Ulid.Empty);
        var validator = new RemoveAssignmentImpedimentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
