namespace Application.Tests.UseCases.Impediment;

public class UpdateImpedimentCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly UpdateImpedimentCommandHandler _handler;
    private readonly List<Domain.Entities.Impediment> _impediments;

    public UpdateImpedimentCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _impediments =
        [
            Domain.Entities.Impediment.Create("Test Impediment 1", Ulid.NewUlid()),
            Domain.Entities.Impediment.Create("Test Impediment 2", Ulid.NewUlid())
        ];

        _dbContextMock.Setup(db => db.Impediments).ReturnsDbSet(_impediments);

        _handler = new UpdateImpedimentCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenImpedimentIsUpdatedSuccessfully()
    {
        // Arrange
        var impediment = _impediments.First();
        var command = new UpdateImpedimentCommand(impediment.Id, "Updated Impediment");

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        Assert.Equal("Updated Impediment", impediment.Name);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var impediment = _impediments.First();
        var command = new UpdateImpedimentCommand(impediment.Id, "Updated Impediment");

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
        var command = new UpdateImpedimentCommand(Ulid.NewUlid(), "Updated Impediment");

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
        var command = new UpdateImpedimentCommand(Ulid.Empty, "Test Impediment");
        var validator = new UpdateImpedimentCommandValidator();

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
        var command = new UpdateImpedimentCommand(Ulid.NewUlid(), string.Empty);
        var validator = new UpdateImpedimentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Name"));
    }
}
