namespace Application.Tests.UseCases.Organization;

public class UpdateOrganizationCommandHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.Organization>> _mockOrganizationRepo;
    private readonly UpdateOrganizationCommandHandler _handler;
    
    public UpdateOrganizationCommandHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockOrganizationRepo = new Mock<IRepository<Domain.Entities.Organization>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.Organization>())
            .Returns(_mockOrganizationRepo.Object);
        
        _handler = new UpdateOrganizationCommandHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenOrganizationIsUpdatedSuccessfully()
    {
        // Arrange
        var organization = Domain.Entities.Organization.Create("Test Organization 1", "Description 1", BaseEntity.GetNewId());
        var command = new UpdateOrganizationCommand(organization.Id, "Updated Test Organization 1", "Updated Description 1");

        var sequence = new MockSequence();

        _mockOrganizationRepo.InSequence(sequence)
            .Setup(r => r.GetByIdAsync(organization.Id))
            .ReturnsAsync(organization)
            .Verifiable();

        _mockOrganizationRepo.InSequence(sequence)
            .Setup(r => r.UpdateAsync(organization))
            .ReturnsAsync(true)
            .Verifiable();

        _mockUnitOfWork.InSequence(sequence)
            .Setup(r => r.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, response);
        _mockOrganizationRepo.Verify(repo => repo.GetByIdAsync(organization.Id), Times.Once);
        _mockOrganizationRepo.Verify(repo => repo.UpdateAsync(organization), Times.Once);
        _mockUnitOfWork.Verify(repo => repo.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var organization = Domain.Entities.Organization.Create("Test Organization 1", "Description 1", BaseEntity.GetNewId());
        var command = new UpdateOrganizationCommand(organization.Id, "Updated Test Organization 1", "Updated Description 1");

        var sequence = new MockSequence();

        _mockOrganizationRepo.InSequence(sequence)
            .Setup(r => r.GetByIdAsync(organization.Id))
            .ReturnsAsync(organization)
            .Verifiable();

        _mockOrganizationRepo.InSequence(sequence)
            .Setup(r => r.UpdateAsync(organization))
            .ReturnsAsync(true)
            .Verifiable();

        _mockUnitOfWork.InSequence(sequence)
            .Setup(r => r.CommitAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"))
            .Verifiable();
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _mockOrganizationRepo.Verify(repo => repo.GetByIdAsync(organization.Id), Times.Once);
        _mockOrganizationRepo.Verify(repo => repo.UpdateAsync(organization), Times.Once);
        _mockUnitOfWork.Verify(repo => repo.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenOrganizationDoesNotExist()
    {
        // Arrange
        var command = new UpdateOrganizationCommand(BaseEntity.GetNewId(), "Updated Test Organization 1", "Updated Description 1");

        _mockOrganizationRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.Organization?)null)
            .Verifiable();

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, response);
        _mockOrganizationRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mockUnitOfWork.Verify(repo => repo.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var command = new UpdateOrganizationCommand(Guid.Empty, "Updated Organization", "Updated Description");
        var validator = new UpdateOrganizationCommandValidator();

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
        var command = new UpdateOrganizationCommand(BaseEntity.GetNewId(), string.Empty, "Updated Description");
        var validator = new UpdateOrganizationCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Name"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenDescriptionIsEmpty()
    {
        // Arrange
        var command = new UpdateOrganizationCommand(BaseEntity.GetNewId(), "Updated Organization", string.Empty);
        var validator = new UpdateOrganizationCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Description"));
    }

    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockOrganizationRepo.Verify();
        GC.SuppressFinalize(this);
    }        
}