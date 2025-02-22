namespace Application.Tests.UseCases.Project;

public class GetProjectByIdQueryHandlerTest : IDisposable
{
    private readonly Mock<IProjectRepository> _mockProjectRepo;
    private readonly GetProjectByIdQueryHandler _handler;
    
    public GetProjectByIdQueryHandlerTest()
    {
        _mockProjectRepo = new Mock<IProjectRepository>();
        
        _handler = new GetProjectByIdQueryHandler(_mockProjectRepo.Object);        
    }

    [Fact]
    public async Task Handle_ShouldReturnProject_WhenProjectExists()
    {
        // Arrange
        var project = Domain.Entities.Project.Create("Test Project", BaseEntity.GetNewId());
        
        _mockProjectRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(project)
            .Verifiable();

        var query = new GetProjectByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.Project);
        _mockProjectRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        // Arrange
        _mockProjectRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1))
            .Verifiable();

        var query = new GetProjectByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.Project);
        _mockProjectRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetProjectByIdQuery(Guid.Empty);
        var validator = new GetProjectByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }

    public void Dispose()
    {
        _mockProjectRepo.Verify();
        GC.SuppressFinalize(this);
    }        
}
