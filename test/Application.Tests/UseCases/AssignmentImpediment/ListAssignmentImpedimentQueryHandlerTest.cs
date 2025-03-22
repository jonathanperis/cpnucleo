namespace Application.Tests.UseCases.AssignmentImpediment;

public class ListAssignmentImpedimentQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.AssignmentImpediment>> _mockAssignmentImpedimentRepo;
    private readonly ListAssignmentImpedimentQueryHandler _handler;

    public ListAssignmentImpedimentQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAssignmentImpedimentRepo = new Mock<IRepository<Domain.Entities.AssignmentImpediment>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.AssignmentImpediment>())
            .Returns(_mockAssignmentImpedimentRepo.Object);
        
        _handler = new ListAssignmentImpedimentQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfAssignmentImpediments_WhenAssignmentImpedimentsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };
        var assignmentImpediments = new List<Domain.Entities.AssignmentImpediment?>
        {
            Domain.Entities.AssignmentImpediment.Create("Test AssignmentImpediment 1", BaseEntity.GetNewId(), BaseEntity.GetNewId()),
            Domain.Entities.AssignmentImpediment.Create("Test AssignmentImpediment 2", BaseEntity.GetNewId(), BaseEntity.GetNewId())
        };

        var paginatedResult = new PaginatedResult<Domain.Entities.AssignmentImpediment?>
        {
            Data = assignmentImpediments,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockAssignmentImpedimentRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListAssignmentImpedimentQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.Result);
        Assert.Equal(assignmentImpediments.Count, response.Result.Data?.Count());
        _mockAssignmentImpedimentRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoAssignmentImpedimentsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.AssignmentImpediment?>
        {
            Data = [],
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockAssignmentImpedimentRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListAssignmentImpedimentQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.NotNull(response.Result.Data);
        Assert.Empty(response.Result.Data);
        _mockAssignmentImpedimentRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListAssignmentImpedimentReturnsNull()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.AssignmentImpediment?>
        {
            Data = null,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockAssignmentImpedimentRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListAssignmentImpedimentQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.Result.Data);
        _mockAssignmentImpedimentRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockAssignmentImpedimentRepo.Verify();
        GC.SuppressFinalize(this);
    }    
}
