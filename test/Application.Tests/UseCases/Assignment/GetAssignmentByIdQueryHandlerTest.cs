namespace Application.Tests.UseCases.Assignment;

public class GetAssignmentByIdQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.Assignment>> _mockAssignmentRepo;
    private readonly GetAssignmentByIdQueryHandler _handler;
    
    public GetAssignmentByIdQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAssignmentRepo = new Mock<IRepository<Domain.Entities.Assignment>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.Assignment>())
            .Returns(_mockAssignmentRepo.Object);
        
        _handler = new GetAssignmentByIdQueryHandler(_mockUnitOfWork.Object);        
    }

    [Fact]
    public async Task Handle_ShouldReturnAssignment_WhenAssignmentExists()
    {
        // Arrange
        var assignment = Domain.Entities.Assignment.Create("Test Assignment", "Assignment Description", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 2, BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId());
        
        _mockAssignmentRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(assignment)
            .Verifiable();

        var query = new GetAssignmentByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.Assignment);
        _mockAssignmentRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenAssignmentDoesNotExist()
    {
        // Arrange
        _mockAssignmentRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1))
            .Verifiable();

        var query = new GetAssignmentByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.Assignment);
        _mockAssignmentRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetAssignmentByIdQuery(Guid.Empty);
        var validator = new GetAssignmentByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }

    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockAssignmentRepo.Verify();
        GC.SuppressFinalize(this);
    }        
}
