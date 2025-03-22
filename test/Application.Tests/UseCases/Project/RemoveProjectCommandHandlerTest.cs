namespace Application.Tests.UseCases.Project;

public class RemoveProjectCommandHandlerTest : IDisposable
{
    private readonly Mock<IProjectRepository> _mockProjectRepo;
    private readonly RemoveProjectCommandHandler _handler;

    public RemoveProjectCommandHandlerTest()
    {
        _mockProjectRepo = new Mock<IProjectRepository>();
        
        _handler = new RemoveProjectCommandHandler(_mockProjectRepo.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProjectIsRemovedSuccessfully()
    {
        // Arrange
        var project = Domain.Entities.Project.Create("Test Project", BaseEntity.GetNewId(), BaseEntity.GetNewId());
        var command = new RemoveProjectCommand(project.Id);

        var sequence = new MockSequence();

        _mockProjectRepo.InSequence(sequence)
            .Setup(r => r.GetByIdAsync(project.Id))
            .ReturnsAsync(project)
            .Verifiable();

        _mockProjectRepo.InSequence(sequence)
            .Setup(r => r.UpdateAsync(project))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        _mockProjectRepo.Verify(repo => repo.GetByIdAsync(project.Id), Times.Once);
        _mockProjectRepo.Verify(repo => repo.UpdateAsync(project), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var project = Domain.Entities.Project.Create("Test Project", BaseEntity.GetNewId(), BaseEntity.GetNewId());
        var organizationId = project.Id;
        var command = new RemoveProjectCommand(organizationId);

        var sequence = new MockSequence();

        _mockProjectRepo.InSequence(sequence)
            .Setup(r => r.GetByIdAsync(organizationId))
            .ReturnsAsync(project)
            .Verifiable();

        _mockProjectRepo.InSequence(sequence)
            .Setup(r => r.UpdateAsync(project))
            .ReturnsAsync(false)
            .Verifiable();
        
        // Act
        var response = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Failed, response);
        _mockProjectRepo.Verify(repo => repo.GetByIdAsync(organizationId), Times.Once);
        _mockProjectRepo.Verify(repo => repo.UpdateAsync(project), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        // Arrange
        var organizationId = BaseEntity.GetNewId();
        var command = new RemoveProjectCommand(organizationId);

        _mockProjectRepo.Setup(r => r.GetByIdAsync(organizationId))
            .ReturnsAsync((Domain.Entities.Project?)null)
            .Verifiable();

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, response);
        _mockProjectRepo.Verify(repo => repo.GetByIdAsync(organizationId), Times.Once);
        _mockProjectRepo.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var command = new RemoveProjectCommand(Guid.Empty);
        var validator = new RemoveProjectCommandValidator();

        // Act
        var result = validator.Validate(command);

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
