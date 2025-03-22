namespace Application.Tests.UseCases.UserProject;

public class GetUserProjectByIdQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.UserProject>> _mockUserProjectRepo;
    private readonly GetUserProjectByIdQueryHandler _handler;
    
    public GetUserProjectByIdQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserProjectRepo = new Mock<IRepository<Domain.Entities.UserProject>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.UserProject>())
            .Returns(_mockUserProjectRepo.Object);
        
        _handler = new GetUserProjectByIdQueryHandler(_mockUnitOfWork.Object);        
    }

    [Fact]
    public async Task Handle_ShouldReturnUserProject_WhenUserProjectExists()
    {
        // Arrange
        var userProject = Domain.Entities.UserProject.Create(BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId());
        
        _mockUserProjectRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(userProject)
            .Verifiable();

        var query = new GetUserProjectByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.UserProject);
        _mockUserProjectRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserProjectDoesNotExist()
    {
        // Arrange
        _mockUserProjectRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1))
            .Verifiable();

        var query = new GetUserProjectByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.UserProject);
        _mockUserProjectRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetUserProjectByIdQuery(Guid.Empty);
        var validator = new GetUserProjectByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }

    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockUserProjectRepo.Verify();
        GC.SuppressFinalize(this);
    }        
}
