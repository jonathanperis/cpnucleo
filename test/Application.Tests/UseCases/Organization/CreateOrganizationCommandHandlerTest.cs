namespace Application.Tests.UseCases.Organization;

public class CreateOrganizationCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly CreateOrganizationCommandHandler _handler;
    private readonly Mock<DbSet<Domain.Entities.Organization>> _mockOrganizationsDbSet;

    public CreateOrganizationCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _mockOrganizationsDbSet = new Mock<DbSet<Domain.Entities.Organization>>();
        _dbContextMock.Setup(db => db.Organizations).Returns(_mockOrganizationsDbSet.Object);

        _handler = new CreateOrganizationCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenOrganizationIsCreatedSuccessfully()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new CreateOrganizationCommand("Test Organization", "Test Description");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        _dbContextMock.Verify(db => db.Organizations!.AddAsync(It.IsAny<Domain.Entities.Organization>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var command = new CreateOrganizationCommand("Test Organization", "Test Description");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.Organizations!.AddAsync(It.IsAny<Domain.Entities.Organization>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateOrganizationCommand(string.Empty, "Test Description");
        var validator = new CreateOrganizationCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Name"));
    }
}
