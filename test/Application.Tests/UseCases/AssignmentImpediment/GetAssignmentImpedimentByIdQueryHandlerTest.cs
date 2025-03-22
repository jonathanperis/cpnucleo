namespace Application.Tests.UseCases.AssignmentImpediment;

public class GetAssignmentImpedimentByIdQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.AssignmentImpediment>> _mockAssignmentImpedimentRepo;
    private readonly GetAssignmentImpedimentByIdQueryHandler _handler;
    
    public GetAssignmentImpedimentByIdQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAssignmentImpedimentRepo = new Mock<IRepository<Domain.Entities.AssignmentImpediment>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.AssignmentImpediment>())
            .Returns(_mockAssignmentImpedimentRepo.Object);
        
        _handler = new GetAssignmentImpedimentByIdQueryHandler(_mockUnitOfWork.Object);        
    }

    [Fact]
    public async Task Handle_ShouldReturnAssignmentImpediment_WhenAssignmentImpedimentExists()
    {
        // Arrange
        var assignmentImpediment = Domain.Entities.AssignmentImpediment.Create("Test AssignmentImpediment", BaseEntity.GetNewId(), BaseEntity.GetNewId());
        
        _mockAssignmentImpedimentRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(assignmentImpediment)
            .Verifiable();

        var query = new GetAssignmentImpedimentByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.AssignmentImpediment);
        _mockAssignmentImpedimentRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenAssignmentImpedimentDoesNotExist()
    {
        // Arrange
        _mockAssignmentImpedimentRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1))
            .Verifiable();

        var query = new GetAssignmentImpedimentByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.AssignmentImpediment);
        _mockAssignmentImpedimentRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetAssignmentImpedimentByIdQuery(Guid.Empty);
        var validator = new GetAssignmentImpedimentByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }

    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockAssignmentImpedimentRepo.Verify();
        GC.SuppressFinalize(this);
    }        
}
