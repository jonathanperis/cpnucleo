namespace Application.Tests.UseCases.User;

public class GetUserByIdQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.User>> _mockUserRepo;
    private readonly GetUserByIdQueryHandler _handler;
    
    public GetUserByIdQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserRepo = new Mock<IRepository<Domain.Entities.User>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.User>())
            .Returns(_mockUserRepo.Object);
        
        _handler = new GetUserByIdQueryHandler(_mockUnitOfWork.Object);        
    }

    [Fact]
    public async Task Handle_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = Domain.Entities.User.Create("Test User", "testUser", "password123", BaseEntity.GetNewId());
        
        _mockUserRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user)
            .Verifiable();

        var query = new GetUserByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.User);
        _mockUserRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        _mockUserRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1))
            .Verifiable();

        var query = new GetUserByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.User);
        _mockUserRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetUserByIdQuery(Guid.Empty);
        var validator = new GetUserByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }

    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockUserRepo.Verify();
        GC.SuppressFinalize(this);
    }        
}
