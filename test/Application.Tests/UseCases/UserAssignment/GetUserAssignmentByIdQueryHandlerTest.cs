namespace Application.Tests.UseCases.UserAssignment;

public class GetUserAssignmentByIdQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.UserAssignment>> _mockUserAssignmentRepo;
    private readonly GetUserAssignmentByIdQueryHandler _handler;
    
    public GetUserAssignmentByIdQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserAssignmentRepo = new Mock<IRepository<Domain.Entities.UserAssignment>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.UserAssignment>())
            .Returns(_mockUserAssignmentRepo.Object);
        
        _handler = new GetUserAssignmentByIdQueryHandler(_mockUnitOfWork.Object);        
    }

    [Fact]
    public async Task Handle_ShouldReturnUserAssignment_WhenUserAssignmentExists()
    {
        // Arrange
        var userAssignment = Domain.Entities.UserAssignment.Create(BaseEntity.GetNewId(), BaseEntity.GetNewId());
        
        _mockUserAssignmentRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(userAssignment)
            .Verifiable();

        var query = new GetUserAssignmentByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.UserAssignment);
        _mockUserAssignmentRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserAssignmentDoesNotExist()
    {
        // Arrange
        _mockUserAssignmentRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1))
            .Verifiable();

        var query = new GetUserAssignmentByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.UserAssignment);
        _mockUserAssignmentRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetUserAssignmentByIdQuery(Guid.Empty);
        var validator = new GetUserAssignmentByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }

    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockUserAssignmentRepo.Verify();
        GC.SuppressFinalize(this);
    }        
}
