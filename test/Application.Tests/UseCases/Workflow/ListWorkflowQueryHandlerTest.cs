namespace Application.Tests.UseCases.Workflow;

public class ListWorkflowQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.Workflow>> _mockWorkflowRepo;
    private readonly ListWorkflowQueryHandler _handler;

    public ListWorkflowQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockWorkflowRepo = new Mock<IRepository<Domain.Entities.Workflow>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.Workflow>())
            .Returns(_mockWorkflowRepo.Object);
        
        _handler = new ListWorkflowQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfWorkflows_WhenWorkflowsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };
        var workflows = new List<Domain.Entities.Workflow?>
        {
            Domain.Entities.Workflow.Create("Test Workflow 1", 1, BaseEntity.GetNewId()),
            Domain.Entities.Workflow.Create("Test Workflow 2", 2, BaseEntity.GetNewId())
        };

        var paginatedResult = new PaginatedResult<Domain.Entities.Workflow?>
        {
            Data = workflows,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockWorkflowRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListWorkflowQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.Result);
        Assert.Equal(workflows.Count, response.Result.Data?.Count());
        _mockWorkflowRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoWorkflowsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.Workflow?>
        {
            Data = [],
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockWorkflowRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListWorkflowQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.NotNull(response.Result.Data);
        Assert.Empty(response.Result.Data);
        _mockWorkflowRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListWorkflowReturnsNull()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.Workflow?>
        {
            Data = null,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockWorkflowRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListWorkflowQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.Result.Data);
        _mockWorkflowRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockWorkflowRepo.Verify();
        GC.SuppressFinalize(this);
    }    
}
