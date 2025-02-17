namespace Application.Tests.UseCases.UserAssignment;

public class ListUserAssignmentsQueryHandlerTest
{
    private readonly Mock<IUserAssignmentRepository> _userAssignmentRepositoryMock;
    private readonly ListUserAssignmentsQueryHandler _handler;

    public ListUserAssignmentsQueryHandlerTest()
    {
        _userAssignmentRepositoryMock = new Mock<IUserAssignmentRepository>();
        _handler = new ListUserAssignmentsQueryHandler(_userAssignmentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfUserAssignments_WhenUserAssignmentsExist()
    {
        // Arrange
        var userAssignments = new List<Domain.Entities.UserAssignment?>
        {
            Domain.Entities.UserAssignment.Create(Guid.NewGuid(), Guid.NewGuid()),
            Domain.Entities.UserAssignment.Create(Guid.NewGuid(), Guid.NewGuid())
        };

        _userAssignmentRepositoryMock
            .Setup(repo => repo.ListUserAssignments())
            .ReturnsAsync(userAssignments);

        var query = new ListUserAssignmentsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.UserAssignments);
        Assert.Equal(userAssignments.Count, result.UserAssignments.Count);
        _userAssignmentRepositoryMock.Verify(repo => repo.ListUserAssignments(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoUserAssignmentsExist()
    {
        // Arrange
        _userAssignmentRepositoryMock
            .Setup(repo => repo.ListUserAssignments())
            .ReturnsAsync([]);

        var query = new ListUserAssignmentsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.UserAssignments);
        Assert.Empty(result.UserAssignments);
        _userAssignmentRepositoryMock.Verify(repo => repo.ListUserAssignments(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListUserAssignmentsReturnsNull()
    {
        // Arrange
        _userAssignmentRepositoryMock
            .Setup(repo => repo.ListUserAssignments())
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new ListUserAssignmentsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.UserAssignments);
        _userAssignmentRepositoryMock.Verify(repo => repo.ListUserAssignments(), Times.Once);
    }
}
