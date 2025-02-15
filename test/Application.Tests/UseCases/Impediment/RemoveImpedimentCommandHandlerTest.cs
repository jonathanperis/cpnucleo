namespace Application.Tests.UseCases.Impediment;

public class RemoveImpedimentCommandHandlerTest
{
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly RemoveImpedimentCommandHandler _handler;
    private readonly List<Domain.Entities.Impediment> _impediments;

    public RemoveImpedimentCommandHandlerTest()
    {
        _dbContextMock = new Mock<ApplicationDbContext>();

        _impediments =
        [
            Domain.Entities.Impediment.Create("Test Impediment 1", Ulid.NewUlid()),
            Domain.Entities.Impediment.Create("Test Impediment 2", Ulid.NewUlid())
        ];

        _dbContextMock.Setup(db => db.Impediments).ReturnsDbSet(_impediments);

        _handler = new RemoveImpedimentCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenImpedimentIsRemovedSuccessfully()
    {
        // Arrange
        var impedimentId = _impediments.First().Id;
        var command = new RemoveImpedimentCommand(impedimentId);

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
        var impedimentId = _impediments.First().Id;
        var command = new RemoveImpedimentCommand(impedimentId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenImpedimentDoesNotExist()
    {
        // Arrange
        var impedimentId = Ulid.NewUlid();
        var command = new RemoveImpedimentCommand(impedimentId);

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
        var command = new RemoveImpedimentCommand(Ulid.Empty);
        var validator = new RemoveImpedimentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
