namespace Application.Tests.UseCases.UserProject;

public class ListUserProjectQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.UserProject>> _mockUserProjectRepo;
    private readonly ListUserProjectQueryHandler _handler;

    public ListUserProjectQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserProjectRepo = new Mock<IRepository<Domain.Entities.UserProject>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.UserProject>())
            .Returns(_mockUserProjectRepo.Object);
        
        _handler = new ListUserProjectQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfUserProjects_WhenUserProjectsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };
        var userProjects = new List<Domain.Entities.UserProject?>
        {
            Domain.Entities.UserProject.Create(BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId()),
            Domain.Entities.UserProject.Create(BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId())
        };

        var paginatedResult = new PaginatedResult<Domain.Entities.UserProject?>
        {
            Data = userProjects,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockUserProjectRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListUserProjectQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.Result);
        Assert.Equal(userProjects.Count, response.Result.Data?.Count());
        _mockUserProjectRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoUserProjectsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.UserProject?>
        {
            Data = [],
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockUserProjectRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListUserProjectQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.NotNull(response.Result.Data);
        Assert.Empty(response.Result.Data);
        _mockUserProjectRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListUserProjectReturnsNull()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.UserProject?>
        {
            Data = null,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockUserProjectRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListUserProjectQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.Result.Data);
        _mockUserProjectRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockUserProjectRepo.Verify();
        GC.SuppressFinalize(this);
    }    
}
