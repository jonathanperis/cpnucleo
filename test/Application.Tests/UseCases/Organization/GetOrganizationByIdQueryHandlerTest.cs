namespace Application.Tests.UseCases.Organization;

public class GetOrganizationByIdQueryHandlerTest
{
    private readonly Mock<IOrganizationRepository> _organizationRepositoryMock;
    private readonly GetOrganizationByIdQueryHandler _handler;

    public GetOrganizationByIdQueryHandlerTest()
    {
        _organizationRepositoryMock = new Mock<IOrganizationRepository>();
        _handler = new GetOrganizationByIdQueryHandler(_organizationRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOrganization_WhenOrganizationExists()
    {
        // Arrange
        var organizationDto = new OrganizationDto("Test Organization", "Test Description")
        {
            Id = Ulid.NewUlid(),
            CreatedAt = DateTime.UtcNow
        };

        _organizationRepositoryMock
            .Setup(repo => repo.GetOrganizationById(It.IsAny<Ulid>()))
            .ReturnsAsync(organizationDto);

        var query = new GetOrganizationByIdQuery(Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Organization);
        _organizationRepositoryMock.Verify(repo => repo.GetOrganizationById(It.IsAny<Ulid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenOrganizationDoesNotExist()
    {
        // Arrange
        _organizationRepositoryMock
            .Setup(repo => repo.GetOrganizationById(It.IsAny<Ulid>()))
            .ReturnsAsync((OrganizationDto?)null);

        var query = new GetOrganizationByIdQuery(Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Organization);
        _organizationRepositoryMock.Verify(repo => repo.GetOrganizationById(It.IsAny<Ulid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetOrganizationByIdQuery(Ulid.Empty);
        var validator = new GetOrganizationByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
