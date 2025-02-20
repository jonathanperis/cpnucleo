namespace Application.Tests.UseCases.AssignmentType;

public class GetAssignmentTypeByIdQueryHandlerTest
{
    private readonly Mock<IAssignmentTypeRepository> _assignmentTypeRepositoryMock;
    private readonly GetAssignmentTypeByIdQueryHandler _handler;

    public GetAssignmentTypeByIdQueryHandlerTest()
    {
        _assignmentTypeRepositoryMock = new Mock<IAssignmentTypeRepository>();
        _handler = new GetAssignmentTypeByIdQueryHandler(_assignmentTypeRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAssignmentType_WhenAssignmentTypeExists()
    {
        // Arrange
        var assignmentType = Domain.Entities.AssignmentType.Create("Test AssignmentType", BaseEntity.GetNewId());

        _assignmentTypeRepositoryMock
            .Setup(repo => repo.GetAssignmentTypeById(It.IsAny<Guid>()))
            .ReturnsAsync(assignmentType);

        var query = new GetAssignmentTypeByIdQuery(BaseEntity.GetNewId());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.AssignmentType);
        _assignmentTypeRepositoryMock.Verify(repo => repo.GetAssignmentTypeById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenAssignmentTypeDoesNotExist()
    {
        // Arrange
        _assignmentTypeRepositoryMock
            .Setup(repo => repo.GetAssignmentTypeById(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new GetAssignmentTypeByIdQuery(BaseEntity.GetNewId());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.AssignmentType);
        _assignmentTypeRepositoryMock.Verify(repo => repo.GetAssignmentTypeById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetAssignmentTypeByIdQuery(Guid.Empty);
        var validator = new GetAssignmentTypeByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
