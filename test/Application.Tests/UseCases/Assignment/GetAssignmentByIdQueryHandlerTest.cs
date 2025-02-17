namespace Application.Tests.UseCases.Assignment;

public class GetAssignmentByIdQueryHandlerTest
{
    private readonly Mock<IAssignmentRepository> _assignmentRepositoryMock;
    private readonly GetAssignmentByIdQueryHandler _handler;

    public GetAssignmentByIdQueryHandlerTest()
    {
        _assignmentRepositoryMock = new Mock<IAssignmentRepository>();
        _handler = new GetAssignmentByIdQueryHandler(_assignmentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAssignment_WhenAssignmentExists()
    {
        // Arrange
        var assignmentDto = new AssignmentDto("Test Assignment", "Assignment Description", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 2, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        _assignmentRepositoryMock
            .Setup(repo => repo.GetAssignmentById(It.IsAny<Guid>()))
            .ReturnsAsync(assignmentDto);

        var query = new GetAssignmentByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Assignment);
        _assignmentRepositoryMock.Verify(repo => repo.GetAssignmentById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenAssignmentDoesNotExist()
    {
        // Arrange
        _assignmentRepositoryMock
            .Setup(repo => repo.GetAssignmentById(It.IsAny<Guid>()))
            .ReturnsAsync((AssignmentDto?)null);

        var query = new GetAssignmentByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Assignment);
        _assignmentRepositoryMock.Verify(repo => repo.GetAssignmentById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetAssignmentByIdQuery(Guid.Empty);
        var validator = new GetAssignmentByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
