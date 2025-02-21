namespace Application.Tests.UseCases.Organization;

public class CreateOrganizationCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.Organization>> _mockOrganizationRepo;
    private readonly CreateOrganizationCommandHandler _handler;
    
    public CreateOrganizationCommandHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockOrganizationRepo = new Mock<IRepository<Domain.Entities.Organization>>();
        
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.Organization>())
            .Returns(_mockOrganizationRepo.Object);
        
        _handler = new CreateOrganizationCommandHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenOrganizationIsCreatedSuccessfully()
    {
        // Arrange
        _mockOrganizationRepo.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Organization>()))
            .ReturnsAsync(Guid.NewGuid())
            .Verifiable();

        _mockUnitOfWork.Setup(r => r.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        
        var query = new CreateOrganizationCommand("Test Organization", "Test Description");

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Success, response);
        _mockUnitOfWork.Verify(repo => repo.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);        
        _mockOrganizationRepo.Verify(repo => repo.AddAsync(It.IsAny<Domain.Entities.Organization>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        _mockOrganizationRepo.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Organization>()))
            .ReturnsAsync(Guid.NewGuid())
            .Verifiable();

        _mockUnitOfWork.Setup(r => r.CommitAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"))
            .Verifiable();
        
        var query = new CreateOrganizationCommand("Test Organization", "Test Description");

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.Equal(OperationResult.Failed, response);
        _mockUnitOfWork.Verify(repo => repo.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);        
        _mockOrganizationRepo.Verify(repo => repo.AddAsync(It.IsAny<Domain.Entities.Organization>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateOrganizationCommand(string.Empty, "Test Description");
        var validator = new CreateOrganizationCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Name"));
    }
}
