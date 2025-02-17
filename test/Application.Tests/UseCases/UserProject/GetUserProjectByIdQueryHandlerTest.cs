namespace Application.Tests.UseCases.UserProject;

public class GetUserProjectByIdQueryHandlerTest
{
    private readonly Mock<IUserProjectRepository> _userProjectRepositoryMock;
    private readonly GetUserProjectByIdQueryHandler _handler;

    public GetUserProjectByIdQueryHandlerTest()
    {
        _userProjectRepositoryMock = new Mock<IUserProjectRepository>();
        _handler = new GetUserProjectByIdQueryHandler(_userProjectRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUserProject_WhenUserProjectExists()
    {
        // Arrange
        var userProject = Domain.Entities.UserProject.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _userProjectRepositoryMock
            .Setup(repo => repo.GetUserProjectById(It.IsAny<Guid>()))
            .ReturnsAsync(userProject);

        var query = new GetUserProjectByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.UserProject);
        _userProjectRepositoryMock.Verify(repo => repo.GetUserProjectById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserProjectDoesNotExist()
    {
        // Arrange
        _userProjectRepositoryMock
            .Setup(repo => repo.GetUserProjectById(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new GetUserProjectByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.UserProject);
        _userProjectRepositoryMock.Verify(repo => repo.GetUserProjectById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetUserProjectByIdQuery(Guid.Empty);
        var validator = new GetUserProjectByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
