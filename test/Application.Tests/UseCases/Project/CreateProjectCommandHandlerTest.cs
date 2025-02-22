namespace Application.Tests.UseCases.Project;

public class CreateProjectCommandHandlerTest : IDisposable
{
    private readonly Mock<IProjectRepository> _mockProjectRepo;
    private readonly CreateProjectCommandHandler _handler;
    
    public CreateProjectCommandHandlerTest()
    {
        _mockProjectRepo = new Mock<IProjectRepository>();
        
        _handler = new CreateProjectCommandHandler(_mockProjectRepo.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProjectIsCreatedSuccessfully()
    {
        // Arrange
        _mockProjectRepo.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Project>()))
            .ReturnsAsync(Guid.NewGuid())
            .Verifiable();
        
        var command = new CreateProjectCommand("Test Project", BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response);
        _mockProjectRepo.Verify(repo => repo.AddAsync(It.IsAny<Domain.Entities.Project>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        _mockProjectRepo.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Project>()))
            .ReturnsAsync(Guid.Empty)
            .Verifiable();
        
        var command = new CreateProjectCommand("Test Project", BaseEntity.GetNewId());

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Failed, response);
        _mockProjectRepo.Verify(repo => repo.AddAsync(It.IsAny<Domain.Entities.Project>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateProjectCommand(string.Empty, BaseEntity.GetNewId());
        var validator = new CreateProjectCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Name"));
    }

    public void Dispose()
    {
        _mockProjectRepo.Verify();
        GC.SuppressFinalize(this);
    }    
}
