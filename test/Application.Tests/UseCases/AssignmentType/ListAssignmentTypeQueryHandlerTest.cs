namespace Application.Tests.UseCases.AssignmentType;

public class ListAssignmentTypeQueryHandlerTest
{
    private readonly Mock<IAssignmentTypeRepository> _assignmentTypeRepositoryMock;
    private readonly ListAssignmentTypeQueryHandler _handler;

    public ListAssignmentTypeQueryHandlerTest()
    {
        _assignmentTypeRepositoryMock = new Mock<IAssignmentTypeRepository>();
        _handler = new ListAssignmentTypeQueryHandler(_assignmentTypeRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfAssignmentTypes_WhenAssignmentTypesExist()
    {
        // Arrange
        var assignmentTypes = new List<Domain.Entities.AssignmentType?>
        {
            Domain.Entities.AssignmentType.Create("Test AssignmentType 1", BaseEntity.GetNewId()),
            Domain.Entities.AssignmentType.Create("Test AssignmentType 2", BaseEntity.GetNewId())
        };

        _assignmentTypeRepositoryMock
            .Setup(repo => repo.ListAssignmentTypes())
            .ReturnsAsync(assignmentTypes);

        var query = new ListAssignmentTypeQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.AssignmentTypes);
        Assert.Equal(assignmentTypes.Count, result.AssignmentTypes.Count);
        _assignmentTypeRepositoryMock.Verify(repo => repo.ListAssignmentTypes(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoAssignmentTypesExist()
    {
        // Arrange
        _assignmentTypeRepositoryMock
            .Setup(repo => repo.ListAssignmentTypes())
            .ReturnsAsync([]);

        var query = new ListAssignmentTypeQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.AssignmentTypes);
        Assert.Empty(result.AssignmentTypes);
        _assignmentTypeRepositoryMock.Verify(repo => repo.ListAssignmentTypes(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListAssignmentTypesReturnsNull()
    {
        // Arrange
        _assignmentTypeRepositoryMock
            .Setup(repo => repo.ListAssignmentTypes())
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new ListAssignmentTypeQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.AssignmentTypes);
        _assignmentTypeRepositoryMock.Verify(repo => repo.ListAssignmentTypes(), Times.Once);
    }
}
