namespace Application.Tests.UseCases.Organization;

public class UpdateOrganizationCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly UpdateOrganizationCommandHandler _handler;
    private readonly List<Domain.Entities.Organization> _organizations;

    public UpdateOrganizationCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _organizations = new List<Domain.Entities.Organization>
        {
            Domain.Entities.Organization.Create("Test Organization 1", "Description 1", Ulid.NewUlid()),
            Domain.Entities.Organization.Create("Test Organization 2", "Description 2", Ulid.NewUlid())
        };

        _dbContextMock.Setup(db => db.Organizations).ReturnsDbSet(_organizations);

        _handler = new UpdateOrganizationCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenOrganizationIsUpdatedSuccessfully()
    {
        // Arrange
        var organization = _organizations.First();
        var command = new UpdateOrganizationCommand(organization.Id, "Updated Organization", "Updated Description");

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        Assert.Equal("Updated Organization", organization.Name);
        Assert.Equal("Updated Description", organization.Description);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var organization = _organizations.First();
        var command = new UpdateOrganizationCommand(organization.Id, "Updated Organization", "Updated Description");

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
        var command = new UpdateOrganizationCommand(Ulid.NewUlid(), "Updated Organization", "Updated Description");

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
        var command = new UpdateOrganizationCommand(Ulid.Empty, "Updated Organization", "Updated Description");
        var validator = new UpdateOrganizationCommandValidator();

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
        var command = new UpdateOrganizationCommand(Ulid.NewUlid(), string.Empty, "Updated Description");
        var validator = new UpdateOrganizationCommandValidator();

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
        var command = new UpdateOrganizationCommand(Ulid.NewUlid(), "Updated Organization", string.Empty);
        var validator = new UpdateOrganizationCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Description"));
    }
}
