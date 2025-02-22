namespace Application.Tests.UseCases.UserAssignment;

public class ListUserAssignmentQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.UserAssignment>> _mockUserAssignmentRepo;
    private readonly ListUserAssignmentQueryHandler _handler;

    public ListUserAssignmentQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserAssignmentRepo = new Mock<IRepository<Domain.Entities.UserAssignment>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.UserAssignment>())
            .Returns(_mockUserAssignmentRepo.Object);
        
        _handler = new ListUserAssignmentQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfUserAssignments_WhenUserAssignmentsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };
        var userAssignments = new List<Domain.Entities.UserAssignment?>
        {
            Domain.Entities.UserAssignment.Create(BaseEntity.GetNewId(), BaseEntity.GetNewId()),
            Domain.Entities.UserAssignment.Create(BaseEntity.GetNewId(), BaseEntity.GetNewId())
        };


        var paginatedResult = new PaginatedResult<Domain.Entities.UserAssignment?>
        {
            Data = userAssignments,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockUserAssignmentRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListUserAssignmentQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.Result);
        Assert.Equal(userAssignments.Count, response.Result.Data?.Count());
        _mockUserAssignmentRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoUserAssignmentsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.UserAssignment?>
        {
            Data = [],
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockUserAssignmentRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListUserAssignmentQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.NotNull(response.Result.Data);
        Assert.Empty(response.Result.Data);
        _mockUserAssignmentRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListUserAssignmentReturnsNull()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.UserAssignment?>
        {
            Data = null,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockUserAssignmentRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListUserAssignmentQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.Result.Data);
        _mockUserAssignmentRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockUserAssignmentRepo.Verify();
        GC.SuppressFinalize(this);
    }    
}
