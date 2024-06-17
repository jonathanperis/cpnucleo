namespace Application.Tests.UseCases.Project;

public class GetProjectByIdQueryHandlerTest
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly GetProjectByIdQueryHandler _handler;

    public GetProjectByIdQueryHandlerTest()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _handler = new GetProjectByIdQueryHandler(_projectRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProject_WhenProjectExists()
    {
        // Arrange
        var projectDto = new ProjectDto("Test Project", Ulid.NewUlid())
        {
            Id = Ulid.NewUlid(),
            CreatedAt = DateTime.UtcNow
        };

        _projectRepositoryMock
            .Setup(repo => repo.GetProjectById(It.IsAny<Ulid>()))
            .ReturnsAsync(projectDto);

        var query = new GetProjectByIdQuery(Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Project);
        _projectRepositoryMock.Verify(repo => repo.GetProjectById(It.IsAny<Ulid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        // Arrange
        _projectRepositoryMock
            .Setup(repo => repo.GetProjectById(It.IsAny<Ulid>()))
            .ReturnsAsync((ProjectDto?)null);

        var query = new GetProjectByIdQuery(Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Project);
        _projectRepositoryMock.Verify(repo => repo.GetProjectById(It.IsAny<Ulid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetProjectByIdQuery(Ulid.Empty);
        var validator = new GetProjectByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
