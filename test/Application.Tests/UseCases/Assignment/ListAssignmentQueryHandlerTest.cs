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
        var assignments = new List<Domain.Entities.Assignment?>
        {
            Domain.Entities.Assignment.Create("Test Assignment 1", "Description 1", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 2, BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId()),
            Domain.Entities.Assignment.Create("Test Assignment 2", "Description 2", DateTime.UtcNow, DateTime.UtcNow.AddDays(2), 3, BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId())
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
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new ListAssignmentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Assignments);
        _assignmentRepositoryMock.Verify(repo => repo.ListAssignments(), Times.Once);
    }
}
