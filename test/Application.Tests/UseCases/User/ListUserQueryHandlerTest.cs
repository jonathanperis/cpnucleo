namespace Application.Tests.UseCases.User;

public class ListUserQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.User>> _mockUserRepo;
    private readonly ListUserQueryHandler _handler;

    public ListUserQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserRepo = new Mock<IRepository<Domain.Entities.User>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.User>())
            .Returns(_mockUserRepo.Object);
        
        _handler = new ListUserQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfUsers_WhenUsersExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };
        var users = new List<Domain.Entities.User?>
        {
            Domain.Entities.User.Create("Test User 1", "testUser1", "password1", BaseEntity.GetNewId()),
            Domain.Entities.User.Create("Test User 2", "testUser2", "password2", BaseEntity.GetNewId())
        };


        var paginatedResult = new PaginatedResult<Domain.Entities.User?>
        {
            Data = users,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockUserRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListUserQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.Result);
        Assert.Equal(users.Count, response.Result.Data?.Count());
        _mockUserRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.User?>
        {
            Data = [],
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockUserRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListUserQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.NotNull(response.Result.Data);
        Assert.Empty(response.Result.Data);
        _mockUserRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListUserReturnsNull()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.User?>
        {
            Data = null,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockUserRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListUserQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.Result.Data);
        _mockUserRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockUserRepo.Verify();
        GC.SuppressFinalize(this);
    }    
}
