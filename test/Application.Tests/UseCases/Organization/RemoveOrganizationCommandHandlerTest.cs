namespace Application.Tests.UseCases.Organization;

public class RemoveOrganizationCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.Organization>> _mockOrganizationRepo;
    private readonly RemoveOrganizationCommandHandler _handler;

    public RemoveOrganizationCommandHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockOrganizationRepo = new Mock<IRepository<Domain.Entities.Organization>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.Organization>())
            .Returns(_mockOrganizationRepo.Object);
        
        _handler = new RemoveOrganizationCommandHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenOrganizationIsRemovedSuccessfully()
    {
        // Arrange
        var organization = Domain.Entities.Organization.Create("Test Organization", "Description", BaseEntity.GetNewId());
        var command = new RemoveOrganizationCommand(organization.Id);

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
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        _mockOrganizationRepo.Verify(repo => repo.GetByIdAsync(organization.Id), Times.Once);
        _mockOrganizationRepo.Verify(repo => repo.UpdateAsync(organization), Times.Once);
        _mockUnitOfWork.Verify(repo => repo.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var organization = Domain.Entities.Organization.Create("Test Organization", "Description", BaseEntity.GetNewId());
        var organizationId = organization.Id;
        var command = new RemoveOrganizationCommand(organizationId);

        var sequence = new MockSequence();

        _mockOrganizationRepo.InSequence(sequence)
            .Setup(r => r.GetByIdAsync(organizationId))
            .ReturnsAsync(organization)
            .Verifiable();

        _mockOrganizationRepo.InSequence(sequence)
            .Setup(r => r.UpdateAsync(organization))
            .ReturnsAsync(false)
            .Verifiable();

        _mockUnitOfWork.InSequence(sequence)
            .Setup(r => r.CommitAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"))
            .Verifiable();
        
        // Act
        var response = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Failed, response);
        _mockOrganizationRepo.Verify(repo => repo.GetByIdAsync(organizationId), Times.Once);
        _mockOrganizationRepo.Verify(repo => repo.UpdateAsync(organization), Times.Once);
        _mockUnitOfWork.Verify(repo => repo.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenOrganizationDoesNotExist()
    {
        // Arrange
        var organizationId = BaseEntity.GetNewId();
        var command = new RemoveOrganizationCommand(organizationId);

        _mockOrganizationRepo.Setup(r => r.GetByIdAsync(organizationId))
            .ReturnsAsync((Domain.Entities.Organization?)null)
            .Verifiable();

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, response);
        _mockOrganizationRepo.Verify(repo => repo.GetByIdAsync(organizationId), Times.Once);
        _mockOrganizationRepo.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        _mockUnitOfWork.Verify(repo => repo.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var command = new RemoveOrganizationCommand(Guid.Empty);
        var validator = new RemoveOrganizationCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}