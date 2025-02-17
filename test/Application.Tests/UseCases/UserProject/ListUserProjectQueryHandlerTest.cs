namespace Application.Tests.UseCases.UserProject;

public class ListUserProjectQueryHandlerTest
{
    private readonly Mock<IUserProjectRepository> _userProjectRepositoryMock;
    private readonly ListUserProjectQueryHandler _handler;

    public ListUserProjectQueryHandlerTest()
    {
        _userProjectRepositoryMock = new Mock<IUserProjectRepository>();
        _handler = new ListUserProjectQueryHandler(_userProjectRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfUserProjects_WhenUserProjectsExist()
    {
        // Arrange
        var userProjects = new List<Domain.Entities.UserProject>
        {
            Domain.Entities.UserProject.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
            Domain.Entities.UserProject.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
        };

        _userProjectRepositoryMock
            .Setup(repo => repo.ListUserProjects())
            .ReturnsAsync(userProjects);

        var query = new ListUserProjectQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.UserProjects);
        Assert.Equal(userProjects.Count, result.UserProjects.Count);
        _userProjectRepositoryMock.Verify(repo => repo.ListUserProjects(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoUserProjectsExist()
    {
        // Arrange
        _userProjectRepositoryMock
            .Setup(repo => repo.ListUserProjects())
            .ReturnsAsync([]);

        var query = new ListUserProjectQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.UserProjects);
        Assert.Empty(result.UserProjects);
        _userProjectRepositoryMock.Verify(repo => repo.ListUserProjects(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListUserProjectsReturnsNull()
    {
        // Arrange
        _userProjectRepositoryMock
            .Setup(repo => repo.ListUserProjects())
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new ListUserProjectQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Empty(result.UserProjects);
        _userProjectRepositoryMock.Verify(repo => repo.ListUserProjects(), Times.Once);
    }
}
