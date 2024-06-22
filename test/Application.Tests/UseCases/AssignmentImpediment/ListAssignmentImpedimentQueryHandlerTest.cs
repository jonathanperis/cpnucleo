namespace Application.Tests.UseCases.AssignmentImpediment;

public class ListAssignmentImpedimentQueryHandlerTest
{
    private readonly Mock<IAssignmentImpedimentRepository> _assignmentImpedimentRepositoryMock;
    private readonly ListAssignmentImpedimentQueryHandler _handler;

    public ListAssignmentImpedimentQueryHandlerTest()
    {
        _assignmentImpedimentRepositoryMock = new Mock<IAssignmentImpedimentRepository>();
        _handler = new ListAssignmentImpedimentQueryHandler(_assignmentImpedimentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfAssignmentImpediments_WhenAssignmentImpedimentsExist()
    {
        // Arrange
        var assignmentImpediments = new List<AssignmentImpedimentDto>
        {
            new AssignmentImpedimentDto("Test AssignmentImpediment 1", Ulid.NewUlid(), Ulid.NewUlid())
            {
                Id = Ulid.NewUlid(),
                CreatedAt = DateTime.UtcNow
            },
            new AssignmentImpedimentDto("Test AssignmentImpediment 2", Ulid.NewUlid(), Ulid.NewUlid())
            {
                Id = Ulid.NewUlid(),
                CreatedAt = DateTime.UtcNow
            }
        };

        _assignmentImpedimentRepositoryMock
            .Setup(repo => repo.ListAssignmentImpediments())
            .ReturnsAsync(assignmentImpediments);

        var query = new ListAssignmentImpedimentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.AssignmentImpediments);
        Assert.Equal(assignmentImpediments.Count, result.AssignmentImpediments.Count);
        _assignmentImpedimentRepositoryMock.Verify(repo => repo.ListAssignmentImpediments(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoAssignmentImpedimentsExist()
    {
        // Arrange
        _assignmentImpedimentRepositoryMock
            .Setup(repo => repo.ListAssignmentImpediments())
            .ReturnsAsync(new List<AssignmentImpedimentDto>());

        var query = new ListAssignmentImpedimentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.AssignmentImpediments);
        Assert.Empty(result.AssignmentImpediments);
        _assignmentImpedimentRepositoryMock.Verify(repo => repo.ListAssignmentImpediments(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListAssignmentImpedimentsReturnsNull()
    {
        // Arrange
        _assignmentImpedimentRepositoryMock
            .Setup(repo => repo.ListAssignmentImpediments())
            .ReturnsAsync((List<AssignmentImpedimentDto>?)null);

        var query = new ListAssignmentImpedimentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Empty(result.AssignmentImpediments);
        _assignmentImpedimentRepositoryMock.Verify(repo => repo.ListAssignmentImpediments(), Times.Once);
    }
}
