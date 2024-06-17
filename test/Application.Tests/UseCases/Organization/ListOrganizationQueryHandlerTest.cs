namespace Application.Tests.UseCases.Organization;

public class ListOrganizationQueryHandlerTest
{
    private readonly Mock<IOrganizationRepository> _organizationRepositoryMock;
    private readonly ListOrganizationQueryHandler _handler;

    public ListOrganizationQueryHandlerTest()
    {
        _organizationRepositoryMock = new Mock<IOrganizationRepository>();
        _handler = new ListOrganizationQueryHandler(_organizationRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfOrganizations_WhenOrganizationsExist()
    {
        // Arrange
        var organizations = new List<OrganizationDto>
        {
            new OrganizationDto("Test Organization 1", "Description 1")
            {
                Id = Ulid.NewUlid(),
                CreatedAt = DateTime.UtcNow
            },
            new OrganizationDto("Test Organization 2", "Description 2")
            {
                Id = Ulid.NewUlid(),
                CreatedAt = DateTime.UtcNow
            }
        };

        _organizationRepositoryMock
            .Setup(repo => repo.ListOrganization())
            .ReturnsAsync(organizations);

        var query = new ListOrganizationQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Organizations);
        Assert.Equal(organizations.Count, result.Organizations.Count);
        _organizationRepositoryMock.Verify(repo => repo.ListOrganization(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoOrganizationsExist()
    {
        // Arrange
        _organizationRepositoryMock
            .Setup(repo => repo.ListOrganization())
            .ReturnsAsync(new List<OrganizationDto>());

        var query = new ListOrganizationQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Organizations);
        Assert.Empty(result.Organizations);
        _organizationRepositoryMock.Verify(repo => repo.ListOrganization(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListOrganizationReturnsNull()
    {
        // Arrange
        _organizationRepositoryMock
            .Setup(repo => repo.ListOrganization())
            .ReturnsAsync((List<OrganizationDto>?)null);

        var query = new ListOrganizationQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Empty(result.Organizations);
        _organizationRepositoryMock.Verify(repo => repo.ListOrganization(), Times.Once);
    }
}
