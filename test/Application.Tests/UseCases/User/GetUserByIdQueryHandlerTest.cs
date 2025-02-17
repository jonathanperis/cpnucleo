namespace Application.Tests.UseCases.User;

public class GetUserByIdQueryHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserByIdQueryHandlerTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetUserByIdQueryHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userDto = new UserDto("Test User", "testUser")
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        _userRepositoryMock
            .Setup(repo => repo.GetUserById(It.IsAny<Guid>()))
            .ReturnsAsync(userDto);

        var query = new GetUserByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.User);
        _userRepositoryMock.Verify(repo => repo.GetUserById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepositoryMock
            .Setup(repo => repo.GetUserById(It.IsAny<Guid>()))
            .ReturnsAsync((UserDto?)null);

        var query = new GetUserByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.User);
        _userRepositoryMock.Verify(repo => repo.GetUserById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetUserByIdQuery(Guid.Empty);
        var validator = new GetUserByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
