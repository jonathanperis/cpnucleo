namespace Application.Tests.UseCases.AssignmentImpediment;

public class CreateAssignmentImpedimentCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly CreateAssignmentImpedimentCommandHandler _handler;

    public CreateAssignmentImpedimentCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        Mock<DbSet<Domain.Entities.AssignmentImpediment>> mockAssignmentImpedimentsDbSet = new();
        _dbContextMock.Setup(db => db.AssignmentImpediments).Returns(mockAssignmentImpedimentsDbSet.Object);

        _handler = new CreateAssignmentImpedimentCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAssignmentImpedimentIsCreatedSuccessfully()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new CreateAssignmentImpedimentCommand("Test AssignmentImpediment", Ulid.NewUlid(), Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        _dbContextMock.Verify(db => db.AssignmentImpediments!.AddAsync(It.IsAny<Domain.Entities.AssignmentImpediment>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var command = new CreateAssignmentImpedimentCommand("Test AssignmentImpediment", Ulid.NewUlid(), Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.AssignmentImpediments!.AddAsync(It.IsAny<Domain.Entities.AssignmentImpediment>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenDescriptionIsEmpty()
    {
        // Arrange
        var command = new CreateAssignmentImpedimentCommand(string.Empty, Ulid.NewUlid(), Ulid.NewUlid());
        var validator = new CreateAssignmentImpedimentCommandValidator();

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
        var command = new CreateAssignmentImpedimentCommand("Test AssignmentImpediment", Ulid.Empty, Ulid.NewUlid());
        var validator = new CreateAssignmentImpedimentCommandValidator();

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
        var command = new CreateAssignmentImpedimentCommand("Test AssignmentImpediment", Ulid.NewUlid(), Ulid.Empty);
        var validator = new CreateAssignmentImpedimentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "ImpedimentId"));
    }
}
