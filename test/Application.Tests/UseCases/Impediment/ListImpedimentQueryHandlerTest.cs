namespace Application.Tests.UseCases.Impediment;

public class ListImpedimentQueryHandlerTest
{
    private readonly Mock<IImpedimentRepository> _impedimentRepositoryMock;
    private readonly ListImpedimentQueryHandler _handler;

    public ListImpedimentQueryHandlerTest()
    {
        _impedimentRepositoryMock = new Mock<IImpedimentRepository>();
        _handler = new ListImpedimentQueryHandler(_impedimentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfImpediments_WhenImpedimentsExist()
    {
        // Arrange
        var impediments = new List<ImpedimentDto>
        {
            new("Test Impediment 1")
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            },
            new("Test Impediment 2")
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            }
        };

        _impedimentRepositoryMock
            .Setup(repo => repo.ListImpediments())
            .ReturnsAsync(impediments);

        var query = new ListImpedimentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Impediments);
        Assert.Equal(impediments.Count, result.Impediments.Count);
        _impedimentRepositoryMock.Verify(repo => repo.ListImpediments(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoImpedimentsExist()
    {
        // Arrange
        _impedimentRepositoryMock
            .Setup(repo => repo.ListImpediments())
            .ReturnsAsync([]);

        var query = new ListImpedimentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Impediments);
        Assert.Empty(result.Impediments);
        _impedimentRepositoryMock.Verify(repo => repo.ListImpediments(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListImpedimentsReturnsNull()
    {
        // Arrange
        _impedimentRepositoryMock
            .Setup(repo => repo.ListImpediments())
            .ReturnsAsync((List<ImpedimentDto>?)null);

        var query = new ListImpedimentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Empty(result.Impediments);
        _impedimentRepositoryMock.Verify(repo => repo.ListImpediments(), Times.Once);
    }
}
