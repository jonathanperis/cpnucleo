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
        var impedimentDto = new ImpedimentDto("Test Impediment")
        {
            Id = Ulid.NewUlid(),
            CreatedAt = DateTime.UtcNow
        };

        _impedimentRepositoryMock
            .Setup(repo => repo.GetImpedimentById(It.IsAny<Ulid>()))
            .ReturnsAsync(impedimentDto);

        var query = new GetImpedimentByIdQuery(Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Impediment);
        _impedimentRepositoryMock.Verify(repo => repo.GetImpedimentById(It.IsAny<Ulid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenImpedimentDoesNotExist()
    {
        // Arrange
        _impedimentRepositoryMock
            .Setup(repo => repo.GetImpedimentById(It.IsAny<Ulid>()))
            .ReturnsAsync((ImpedimentDto?)null);

        var query = new GetImpedimentByIdQuery(Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Impediment);
        _impedimentRepositoryMock.Verify(repo => repo.GetImpedimentById(It.IsAny<Ulid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetImpedimentByIdQuery(Ulid.Empty);
        var validator = new GetImpedimentByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
