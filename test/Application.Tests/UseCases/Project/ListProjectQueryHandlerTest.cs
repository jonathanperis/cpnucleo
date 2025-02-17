namespace Application.Tests.UseCases.Project;

public class ListProjectQueryHandlerTest
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly ListProjectQueryHandler _handler;

    public ListProjectQueryHandlerTest()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _handler = new ListProjectQueryHandler(_projectRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfProjects_WhenProjectsExist()
    {
        // Arrange
        var projects = new List<Domain.Entities.Project> 
        { 
            Domain.Entities.Project.Create("Test Project 1", Guid.NewGuid()), 
            Domain.Entities.Project.Create("Test Project 2", Guid.NewGuid()) 
        };

        _projectRepositoryMock
            .Setup(repo => repo.ListProjects())
            .ReturnsAsync(projects);

        var query = new ListProjectQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Projects);
        Assert.Equal(projects.Count, result.Projects.Count);
        _projectRepositoryMock.Verify(repo => repo.ListProjects(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoProjectsExist()
    {
        // Arrange
        _projectRepositoryMock
            .Setup(repo => repo.ListProjects())
            .ReturnsAsync([]);

        var query = new ListProjectQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Projects);
        Assert.Empty(result.Projects);
        _projectRepositoryMock.Verify(repo => repo.ListProjects(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListProjectsReturnsNull()
    {
        // Arrange
        _projectRepositoryMock
            .Setup(repo => repo.ListProjects())
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new ListProjectQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Empty(result.Projects);
        _projectRepositoryMock.Verify(repo => repo.ListProjects(), Times.Once);
    }
}
