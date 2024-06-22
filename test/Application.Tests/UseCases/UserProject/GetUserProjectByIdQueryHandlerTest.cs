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
        var userProjectDto = new UserProjectDto(Ulid.NewUlid(), Ulid.NewUlid())
        {
            Id = Ulid.NewUlid(),
            CreatedAt = DateTime.UtcNow
        };

        _userProjectRepositoryMock
            .Setup(repo => repo.GetUserProjectById(It.IsAny<Ulid>()))
            .ReturnsAsync(userProjectDto);

        var query = new GetUserProjectByIdQuery(Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.UserProject);
        _userProjectRepositoryMock.Verify(repo => repo.GetUserProjectById(It.IsAny<Ulid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserProjectDoesNotExist()
    {
        // Arrange
        _userProjectRepositoryMock
            .Setup(repo => repo.GetUserProjectById(It.IsAny<Ulid>()))
            .ReturnsAsync((UserProjectDto?)null);

        var query = new GetUserProjectByIdQuery(Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.UserProject);
        _userProjectRepositoryMock.Verify(repo => repo.GetUserProjectById(It.IsAny<Ulid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetUserProjectByIdQuery(Ulid.Empty);
        var validator = new GetUserProjectByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
