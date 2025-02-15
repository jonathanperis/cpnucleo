namespace Application.Tests.UseCases.Organization;

public class RemoveOrganizationCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly RemoveOrganizationCommandHandler _handler;
    private readonly List<Domain.Entities.Organization> _organizations;

    public RemoveOrganizationCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _organizations =
        [
            Domain.Entities.Organization.Create("Test Organization 1", "Description 1", Ulid.NewUlid()),
            Domain.Entities.Organization.Create("Test Organization 2", "Description 2", Ulid.NewUlid())
        ];

        _dbContextMock.Setup(db => db.Organizations).ReturnsDbSet(_organizations);

        _handler = new RemoveOrganizationCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenOrganizationIsRemovedSuccessfully()
    {
        // Arrange
        var organizationId = _organizations.First().Id;
        var command = new RemoveOrganizationCommand(organizationId);

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
        var organizationId = _organizations.First().Id;
        var command = new RemoveOrganizationCommand(organizationId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenOrganizationDoesNotExist()
    {
        // Arrange
        var organizationId = Ulid.NewUlid();
        var command = new RemoveOrganizationCommand(organizationId);

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
        var command = new RemoveOrganizationCommand(Ulid.Empty);
        var validator = new RemoveOrganizationCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
