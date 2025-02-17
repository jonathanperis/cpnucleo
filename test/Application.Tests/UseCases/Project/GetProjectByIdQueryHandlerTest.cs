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
        var projectEntity = Domain.Entities.Project.Create("Test Project", Guid.NewGuid());
        
        _projectRepositoryMock
            .Setup(repo => repo.GetProjectById(It.IsAny<Guid>()))
            .ReturnsAsync(projectEntity);

        var query = new GetProjectByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Project);
        _projectRepositoryMock.Verify(repo => repo.GetProjectById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        // Arrange
        _projectRepositoryMock
            .Setup(repo => repo.GetProjectById(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new GetProjectByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Project);
        _projectRepositoryMock.Verify(repo => repo.GetProjectById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetProjectByIdQuery(Guid.Empty);
        var validator = new GetProjectByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
