namespace Application.Tests.UseCases.AssignmentImpediment;

public class GetAssignmentImpedimentByIdQueryHandlerTest
{
    private readonly Mock<IAssignmentImpedimentRepository> _assignmentImpedimentRepositoryMock;
    private readonly GetAssignmentImpedimentByIdQueryHandler _handler;

    public GetAssignmentImpedimentByIdQueryHandlerTest()
    {
        _assignmentImpedimentRepositoryMock = new Mock<IAssignmentImpedimentRepository>();
        _handler = new GetAssignmentImpedimentByIdQueryHandler(_assignmentImpedimentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAssignmentImpediment_WhenAssignmentImpedimentExists()
    {
        // Arrange
        var assignmentImpediment = Domain.Entities.AssignmentImpediment.Create("Test AssignmentImpediment", Guid.NewGuid(), Guid.NewGuid());

        _assignmentImpedimentRepositoryMock
            .Setup(repo => repo.GetAssignmentImpedimentById(It.IsAny<Guid>()))
            .ReturnsAsync(assignmentImpediment);

        var query = new GetAssignmentImpedimentByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.AssignmentImpediment);
        _assignmentImpedimentRepositoryMock.Verify(repo => repo.GetAssignmentImpedimentById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenAssignmentImpedimentDoesNotExist()
    {
        // Arrange
        _assignmentImpedimentRepositoryMock
            .Setup(repo => repo.GetAssignmentImpedimentById(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new GetAssignmentImpedimentByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.AssignmentImpediment);
        _assignmentImpedimentRepositoryMock.Verify(repo => repo.GetAssignmentImpedimentById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetAssignmentImpedimentByIdQuery(Guid.Empty);
        var validator = new GetAssignmentImpedimentByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
