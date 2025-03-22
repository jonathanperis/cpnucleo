namespace Application.Tests.UseCases.Workflow;

public class GetWorkflowByIdQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.Workflow>> _mockWorkflowRepo;
    private readonly GetWorkflowByIdQueryHandler _handler;
    
    public GetWorkflowByIdQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockWorkflowRepo = new Mock<IRepository<Domain.Entities.Workflow>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.Workflow>())
            .Returns(_mockWorkflowRepo.Object);
        
        _handler = new GetWorkflowByIdQueryHandler(_mockUnitOfWork.Object);        
    }

    [Fact]
    public async Task Handle_ShouldReturnWorkflow_WhenWorkflowExists()
    {
        // Arrange
        var workflow = Domain.Entities.Workflow.Create("Test Workflow", 1, BaseEntity.GetNewId());
        
        _mockWorkflowRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(workflow)
            .Verifiable();

        var query = new GetWorkflowByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response.OperationResult);
        Assert.NotNull(response.Workflow);
        _mockWorkflowRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenWorkflowDoesNotExist()
    {
        // Arrange
        _mockWorkflowRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1))
            .Verifiable();

        var query = new GetWorkflowByIdQuery(BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.NotFound, response.OperationResult);
        Assert.Null(response.Workflow);
        _mockWorkflowRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetWorkflowByIdQuery(Guid.Empty);
        var validator = new GetWorkflowByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }

    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockWorkflowRepo.Verify();
        GC.SuppressFinalize(this);
    }        
}
