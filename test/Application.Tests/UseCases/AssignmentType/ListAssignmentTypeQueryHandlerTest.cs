namespace Application.Tests.UseCases.AssignmentType;

public class ListAssignmentTypeQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.AssignmentType>> _mockAssignmentTypeRepo;
    private readonly ListAssignmentTypeQueryHandler _handler;

    public ListAssignmentTypeQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAssignmentTypeRepo = new Mock<IRepository<Domain.Entities.AssignmentType>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.AssignmentType>())
            .Returns(_mockAssignmentTypeRepo.Object);
        
        _handler = new ListAssignmentTypeQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfAssignmentTypes_WhenAssignmentTypesExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };
        var assignmentTypes = new List<Domain.Entities.AssignmentType?>
        {
            Domain.Entities.AssignmentType.Create("Test AssignmentType 1", BaseEntity.GetNewId()),
            Domain.Entities.AssignmentType.Create("Test AssignmentType 2", BaseEntity.GetNewId())
        };

        var paginatedResult = new PaginatedResult<Domain.Entities.AssignmentType?>
        {
            Data = assignmentTypes,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockAssignmentTypeRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListAssignmentTypeQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.Result);
        Assert.Equal(assignmentTypes.Count, response.Result.Data?.Count());
        _mockAssignmentTypeRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoAssignmentTypesExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.AssignmentType?>
        {
            Data = [],
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockAssignmentTypeRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListAssignmentTypeQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.NotNull(response.Result.Data);
        Assert.Empty(response.Result.Data);
        _mockAssignmentTypeRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListAssignmentTypeReturnsNull()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.AssignmentType?>
        {
            Data = null,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockAssignmentTypeRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListAssignmentTypeQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.Result.Data);
        _mockAssignmentTypeRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockAssignmentTypeRepo.Verify();
        GC.SuppressFinalize(this);
    }    
}
