namespace Application.Tests.UseCases.AssignmentType;

public class CreateAssignmentTypeCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly CreateAssignmentTypeCommandHandler _handler;
    private readonly Mock<DbSet<Domain.Entities.AssignmentType>> _mockAssignmentTypesDbSet;

    public CreateAssignmentTypeCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _mockAssignmentTypesDbSet = new Mock<DbSet<Domain.Entities.AssignmentType>>();
        _dbContextMock.Setup(db => db.AssignmentTypes).Returns(_mockAssignmentTypesDbSet.Object);

        _handler = new CreateAssignmentTypeCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAssignmentTypeIsCreatedSuccessfully()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new CreateAssignmentTypeCommand("Test AssignmentType");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        _dbContextMock.Verify(db => db.AssignmentTypes!.AddAsync(It.IsAny<Domain.Entities.AssignmentType>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var command = new CreateAssignmentTypeCommand("Test AssignmentType");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.AssignmentTypes!.AddAsync(It.IsAny<Domain.Entities.AssignmentType>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateAssignmentTypeCommand(string.Empty);
        var validator = new CreateAssignmentTypeCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Name"));
    }
}
