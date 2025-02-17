namespace Application.Tests.UseCases.Impediment;

public class GetImpedimentByIdQueryHandlerTest
{
    private readonly Mock<IImpedimentRepository> _impedimentRepositoryMock;
    private readonly GetImpedimentByIdQueryHandler _handler;

    public GetImpedimentByIdQueryHandlerTest()
    {
        _impedimentRepositoryMock = new Mock<IImpedimentRepository>();
        _handler = new GetImpedimentByIdQueryHandler(_impedimentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnImpediment_WhenImpedimentExists()
    {
        // Arrange
        var impediment = Domain.Entities.Impediment.Create("Test Impediment", Guid.NewGuid());

        _impedimentRepositoryMock
            .Setup(repo => repo.GetImpedimentById(It.IsAny<Guid>()))
            .ReturnsAsync(impediment);

        var query = new GetImpedimentByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Impediment);
        _impedimentRepositoryMock.Verify(repo => repo.GetImpedimentById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenImpedimentDoesNotExist()
    {
        // Arrange
        _impedimentRepositoryMock
            .Setup(repo => repo.GetImpedimentById(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new GetImpedimentByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Impediment);
        _impedimentRepositoryMock.Verify(repo => repo.GetImpedimentById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetImpedimentByIdQuery(Guid.Empty);
        var validator = new GetImpedimentByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
