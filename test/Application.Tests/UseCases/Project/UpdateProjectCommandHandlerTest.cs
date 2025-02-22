namespace Application.Tests.UseCases.Project;

public class UpdateProjectCommandHandlerTest : IDisposable
{
    private readonly Mock<IProjectRepository> _mockProjectRepo;
    private readonly UpdateProjectCommandHandler _handler;
    
    public UpdateProjectCommandHandlerTest()
    {
        _mockProjectRepo = new Mock<IProjectRepository>();
        
        _handler = new UpdateProjectCommandHandler(_mockProjectRepo.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProjectIsUpdatedSuccessfully()
    {
        // Arrange
        var organization = Domain.Entities.Project.Create("Test Project 1", BaseEntity.GetNewId());
        var command = new UpdateProjectCommand(organization.Id, "Updated Test Project 1", BaseEntity.GetNewId());

        var sequence = new MockSequence();

        _mockProjectRepo.InSequence(sequence)
            .Setup(r => r.GetByIdAsync(organization.Id))
            .ReturnsAsync(organization)
            .Verifiable();

        _mockProjectRepo.InSequence(sequence)
            .Setup(r => r.UpdateAsync(organization))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, response);
        _mockProjectRepo.Verify(repo => repo.GetByIdAsync(organization.Id), Times.Once);
        _mockProjectRepo.Verify(repo => repo.UpdateAsync(organization), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var organization = Domain.Entities.Project.Create("Test Project 1", BaseEntity.GetNewId());
        var command = new UpdateProjectCommand(organization.Id, "Updated Test Project 1", BaseEntity.GetNewId());

        var sequence = new MockSequence();

        _mockProjectRepo.InSequence(sequence)
            .Setup(r => r.GetByIdAsync(organization.Id))
            .ReturnsAsync(organization)
            .Verifiable();

        _mockProjectRepo.InSequence(sequence)
            .Setup(r => r.UpdateAsync(organization))
            .ReturnsAsync(false)
            .Verifiable();
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _mockProjectRepo.Verify(repo => repo.GetByIdAsync(organization.Id), Times.Once);
        _mockProjectRepo.Verify(repo => repo.UpdateAsync(organization), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        // Arrange
        var command = new UpdateProjectCommand(BaseEntity.GetNewId(), "Updated Test Project 1", BaseEntity.GetNewId());

        _mockProjectRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.Project?)null)
            .Verifiable();

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, response);
        _mockProjectRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var command = new UpdateProjectCommand(Guid.Empty, "Updated Project", BaseEntity.GetNewId());
        var validator = new UpdateProjectCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenNameIsEmpty()
    {
        // Arrange
        var command = new UpdateProjectCommand(BaseEntity.GetNewId(), string.Empty, BaseEntity.GetNewId());
        var validator = new UpdateProjectCommandValidator();

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