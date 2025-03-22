namespace Application.Tests.UseCases.AssignmentType;

public class GetAssignmentTypeByIdQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.AssignmentType>> _mockAssignmentTypeRepo;
    private readonly GetAssignmentTypeByIdQueryHandler _handler;
    
    public GetAssignmentTypeByIdQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAssignmentTypeRepo = new Mock<IRepository<Domain.Entities.AssignmentType>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.AssignmentType>())
            .Returns(_mockAssignmentTypeRepo.Object);
        
        _handler = new GetAssignmentTypeByIdQueryHandler(_mockUnitOfWork.Object);        
    }

    [Fact]
    public async Task Handle_ShouldReturnAssignmentType_WhenAssignmentTypeExists()
    {
        // Arrange
        var assignmentType = Domain.Entities.AssignmentType.Create("Test AssignmentType", BaseEntity.GetNewId());
        
        _mockAssignmentTypeRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(assignmentType)
            .Verifiable();

        var query = new GetAssignmentTypeByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.AssignmentType);
        _mockAssignmentTypeRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenAssignmentTypeDoesNotExist()
    {
        // Arrange
        _mockAssignmentTypeRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1))
            .Verifiable();

        var query = new GetAssignmentTypeByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.AssignmentType);
        _mockAssignmentTypeRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetAssignmentTypeByIdQuery(Guid.Empty);
        var validator = new GetAssignmentTypeByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }

    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockAssignmentTypeRepo.Verify();
        GC.SuppressFinalize(this);
    }        
}
