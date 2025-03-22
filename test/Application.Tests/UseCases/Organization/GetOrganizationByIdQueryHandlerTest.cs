namespace Application.Tests.UseCases.Organization;

public class GetOrganizationByIdQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.Organization>> _mockOrganizationRepo;
    private readonly GetOrganizationByIdQueryHandler _handler;
    
    public GetOrganizationByIdQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockOrganizationRepo = new Mock<IRepository<Domain.Entities.Organization>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.Organization>())
            .Returns(_mockOrganizationRepo.Object);
        
        _handler = new GetOrganizationByIdQueryHandler(_mockUnitOfWork.Object);        
    }

    [Fact]
    public async Task Handle_ShouldReturnOrganization_WhenOrganizationExists()
    {
        // Arrange
        var organization = Domain.Entities.Organization.Create("Test Organization", "Test Description", BaseEntity.GetNewId());
        
        _mockOrganizationRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(organization)
            .Verifiable();

        var query = new GetOrganizationByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.Organization);
        _mockOrganizationRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenOrganizationDoesNotExist()
    {
        // Arrange
        _mockOrganizationRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1))
            .Verifiable();

        var query = new GetOrganizationByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.Organization);
        _mockOrganizationRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetOrganizationByIdQuery(Guid.Empty);
        var validator = new GetOrganizationByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }

    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockOrganizationRepo.Verify();
        GC.SuppressFinalize(this);
    }        
}
