namespace Application.Tests.UseCases.Project;

public class ListProjectQueryHandlerTest : IDisposable
{
    private readonly Mock<IProjectRepository> _mockProjectRepo;
    private readonly ListProjectQueryHandler _handler;

    public ListProjectQueryHandlerTest()
    {
        _mockProjectRepo = new Mock<IProjectRepository>();
        
        _handler = new ListProjectQueryHandler(_mockProjectRepo.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfProjects_WhenProjectsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };
        var projects = new List<Domain.Entities.Project?> 
        { 
            Domain.Entities.Project.Create("Test Project 1", BaseEntity.GetNewId()), 
            Domain.Entities.Project.Create("Test Project 2", BaseEntity.GetNewId()) 
        };


        var paginatedResult = new PaginatedResult<Domain.Entities.Project?>
        {
            Data = projects,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockProjectRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListProjectQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.Result);
        Assert.Equal(projects.Count, response.Result.Data?.Count());
        _mockProjectRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoProjectsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.Project?>
        {
            Data = [],
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockProjectRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListProjectQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.NotNull(response.Result.Data);
        Assert.Empty(response.Result.Data);
        _mockProjectRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListProjectReturnsNull()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var paginatedResult = new PaginatedResult<Domain.Entities.Project?>
        {
            Data = null,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _mockProjectRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListProjectQuery(pagination);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.Result.Data);
        _mockProjectRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }
    
    public void Dispose()
    {
        _mockProjectRepo.Verify();
        GC.SuppressFinalize(this);
    }    
}
