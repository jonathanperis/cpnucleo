namespace Application.Tests.UseCases.Project;

public class UpdateProjectCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly UpdateProjectCommandHandler _handler;
    private readonly List<Domain.Entities.Project> _projects;

    public UpdateProjectCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _projects =
        [
            Domain.Entities.Project.Create("Test Project 1", Guid.NewGuid(), Guid.NewGuid()),
            Domain.Entities.Project.Create("Test Project 2", Guid.NewGuid(), Guid.NewGuid())
        ];

        _dbContextMock.Setup(db => db.Projects).ReturnsDbSet(_projects);

        _handler = new UpdateProjectCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProjectIsUpdatedSuccessfully()
    {
        // Arrange
        var project = _projects.First();
        var command = new UpdateProjectCommand(project.Id, "Updated Project", project.OrganizationId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        Assert.Equal("Updated Project", project.Name);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var project = _projects.First();
        var command = new UpdateProjectCommand(project.Id, "Updated Project", project.OrganizationId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        // Arrange
        var command = new UpdateProjectCommand(Guid.NewGuid(), "Updated Project", Guid.NewGuid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var command = new UpdateProjectCommand(Guid.Empty, "Test Project", Guid.NewGuid());
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
        var command = new UpdateProjectCommand(Guid.NewGuid(), string.Empty, Guid.NewGuid());
        var validator = new UpdateProjectCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Name"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenOrganizationIdIsEmpty()
    {
        // Arrange
        var command = new UpdateProjectCommand(Guid.NewGuid(), "Test Project", Guid.Empty);
        var validator = new UpdateProjectCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "OrganizationId"));
    }
}
