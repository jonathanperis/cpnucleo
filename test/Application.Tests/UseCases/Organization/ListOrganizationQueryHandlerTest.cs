namespace Application.Tests.UseCases.Organization;

public class ListOrganizationQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.Organization>> _mockOrganizationRepo;
    private readonly ListOrganizationQueryHandler _handler;

    public ListOrganizationQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockOrganizationRepo = new Mock<IRepository<Domain.Entities.Organization>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.Organization>())
            .Returns(_mockOrganizationRepo.Object);
        
        _handler = new ListOrganizationQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfOrganizations_WhenOrganizationsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };
        var organizations = new List<Domain.Entities.Organization>
        {
            Domain.Entities.Organization.Create("Test Organization 1", "Description 1", BaseEntity.GetNewId()),
            Domain.Entities.Organization.Create("Test Organization 2", "Description 2", BaseEntity.GetNewId())
        };

        var paginatedResult = new PaginatedResult<Domain.Entities.Organization?>
        {
            Data = organizations,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockOrganizationRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListOrganizationQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.Result);
        Assert.Equal(organizations.Count, response.Result.Data?.Count());
        _mockOrganizationRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoOrganizationsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.Organization?>
        {
            Data = [],
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockOrganizationRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListOrganizationQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.NotNull(response.Result.Data);
        Assert.Empty(response.Result.Data);
        _mockOrganizationRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListOrganizationReturnsNull()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.Organization?>
        {
            Data = null,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockOrganizationRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListOrganizationQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.Result.Data);
        _mockOrganizationRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockOrganizationRepo.Verify();
        GC.SuppressFinalize(this);
    }    
}
