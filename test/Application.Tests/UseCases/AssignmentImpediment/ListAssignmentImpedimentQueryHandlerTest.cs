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
        var assignmentImpediments = new List<Domain.Entities.AssignmentImpediment?>
        {
            Domain.Entities.AssignmentImpediment.Create("Test AssignmentImpediment 1", BaseEntity.GetNewId(), BaseEntity.GetNewId()),
            Domain.Entities.AssignmentImpediment.Create("Test AssignmentImpediment 2", BaseEntity.GetNewId(), BaseEntity.GetNewId())
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
            .ReturnsAsync([]);

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
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new ListAssignmentImpedimentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.AssignmentImpediments);
        _assignmentImpedimentRepositoryMock.Verify(repo => repo.ListAssignmentImpediments(), Times.Once);
    }
}
