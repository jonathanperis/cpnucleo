namespace Application.Tests.UseCases.Assignment;

public class ListAssignmentQueryHandlerTest
{
    private readonly Mock<IAssignmentRepository> _assignmentRepositoryMock;
    private readonly ListAssignmentQueryHandler _handler;

    public ListAssignmentQueryHandlerTest()
    {
        _assignmentRepositoryMock = new Mock<IAssignmentRepository>();
        _handler = new ListAssignmentQueryHandler(_assignmentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfAssignments_WhenAssignmentsExist()
    {
        // Arrange
        var assignments = new List<AssignmentDto>
        {
            new("Test Assignment 1", "Description 1", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 2, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            },
            new("Test Assignment 2", "Description 2", DateTime.UtcNow, DateTime.UtcNow.AddDays(2), 3, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            }
        };

        _assignmentRepositoryMock
            .Setup(repo => repo.ListAssignments())
            .ReturnsAsync(assignments);

        var query = new ListAssignmentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Assignments);
        Assert.Equal(assignments.Count, result.Assignments.Count);
        _assignmentRepositoryMock.Verify(repo => repo.ListAssignments(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoAssignmentsExist()
    {
        // Arrange
        _assignmentRepositoryMock
            .Setup(repo => repo.ListAssignments())
            .ReturnsAsync([]);

        var query = new ListAssignmentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Assignments);
        Assert.Empty(result.Assignments);
        _assignmentRepositoryMock.Verify(repo => repo.ListAssignments(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListAssignmentsReturnsNull()
    {
        // Arrange
        _assignmentRepositoryMock
            .Setup(repo => repo.ListAssignments())
            .ReturnsAsync((List<AssignmentDto>?)null);

        var query = new ListAssignmentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Empty(result.Assignments);
        _assignmentRepositoryMock.Verify(repo => repo.ListAssignments(), Times.Once);
    }
}
