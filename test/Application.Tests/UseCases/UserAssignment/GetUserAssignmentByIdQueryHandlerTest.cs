namespace Application.Tests.UseCases.UserAssignment;

public class GetUserAssignmentByIdQueryHandlerTest
{
    private readonly Mock<IUserAssignmentRepository> _userAssignmentRepositoryMock;
    private readonly GetUserAssignmentByIdQueryHandler _handler;

    public GetUserAssignmentByIdQueryHandlerTest()
    {
        _userAssignmentRepositoryMock = new Mock<IUserAssignmentRepository>();
        _handler = new GetUserAssignmentByIdQueryHandler(_userAssignmentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUserAssignment_WhenUserAssignmentExists()
    {
        // Arrange
        var userAssignment = Domain.Entities.UserAssignment.Create(Guid.NewGuid(), Guid.NewGuid());

        _userAssignmentRepositoryMock
            .Setup(repo => repo.GetUserAssignmentById(It.IsAny<Guid>()))
            .ReturnsAsync(userAssignment);

        var query = new GetUserAssignmentByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.UserAssignment);
        _userAssignmentRepositoryMock.Verify(repo => repo.GetUserAssignmentById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserAssignmentDoesNotExist()
    {
        // Arrange
        _userAssignmentRepositoryMock
            .Setup(repo => repo.GetUserAssignmentById(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new GetUserAssignmentByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.UserAssignment);
        _userAssignmentRepositoryMock.Verify(repo => repo.GetUserAssignmentById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetUserAssignmentByIdQuery(Guid.Empty);
        var validator = new GetUserAssignmentByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
