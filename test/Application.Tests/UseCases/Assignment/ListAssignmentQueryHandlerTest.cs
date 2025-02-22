namespace Application.Tests.UseCases.Assignment;

public class ListAssignmentQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.Assignment>> _mockAssignmentRepo;
    private readonly ListAssignmentQueryHandler _handler;

    public ListAssignmentQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAssignmentRepo = new Mock<IRepository<Domain.Entities.Assignment>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.Assignment>())
            .Returns(_mockAssignmentRepo.Object);
        
        _handler = new ListAssignmentQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfAssignments_WhenAssignmentsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };
        var assignments = new List<Domain.Entities.Assignment?>
        {
            Domain.Entities.Assignment.Create("Test Assignment 1", "Description 1", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 2, BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId()),
            Domain.Entities.Assignment.Create("Test Assignment 2", "Description 2", DateTime.UtcNow, DateTime.UtcNow.AddDays(2), 3, BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId())
        };

        var paginatedResult = new PaginatedResult<Domain.Entities.Assignment?>
        {
            Data = assignments,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockAssignmentRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListAssignmentQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.Result);
        Assert.Equal(assignments.Count, response.Result.Data?.Count());
        _mockAssignmentRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoAssignmentsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.Assignment?>
        {
            Data = [],
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockAssignmentRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListAssignmentQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.NotNull(response.Result.Data);
        Assert.Empty(response.Result.Data);
        _mockAssignmentRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListAssignmentReturnsNull()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.Assignment?>
        {
            Data = null,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockAssignmentRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListAssignmentQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.Result.Data);
        _mockAssignmentRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockAssignmentRepo.Verify();
        GC.SuppressFinalize(this);
    }    
}
