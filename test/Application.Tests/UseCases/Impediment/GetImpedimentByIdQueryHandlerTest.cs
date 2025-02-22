namespace Application.Tests.UseCases.Impediment;

public class GetImpedimentByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnImpediment_WhenImpedimentExists()
    {
        // Arrange
        var impedimentId = Guid.NewGuid();
        var mockImpediment = Domain.Entities.Impediment.Create("Test Impediment", impedimentId);
        
        var mockDbContext = CreateMockDbContextWithData([mockImpediment]);
        var handler = new GetImpedimentByIdQueryHandler(mockDbContext.Object);
        var request = new GetImpedimentByIdQuery(impedimentId);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Impediment);
        Assert.Equal(impedimentId, result.Impediment.Id);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenImpedimentDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var mockDbContext = CreateMockDbContextWithData(new List<Domain.Entities.Impediment>());
        var handler = new GetImpedimentByIdQueryHandler(mockDbContext.Object);
        var request = new GetImpedimentByIdQuery(nonExistentId);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Impediment);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenIdIsEmpty()
    {
        // Arrange
        var emptyId = Guid.Empty;
        var mockDbContext = CreateMockDbContextWithData(new List<Domain.Entities.Impediment>());
        var handler = new GetImpedimentByIdQueryHandler(mockDbContext.Object);
        var request = new GetImpedimentByIdQuery(emptyId);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Impediment);
    }

    private static Mock<IApplicationDbContext> CreateMockDbContextWithData(List<Domain.Entities.Impediment> impediments)
    {
        var mockDbContext = new Mock<IApplicationDbContext>();
        var data = impediments.AsQueryable();

        var mockDbSet = new Mock<DbSet<Domain.Entities.Impediment>>();
        
        mockDbSet.As<IQueryable<Domain.Entities.Impediment>>().Setup(m => m.Provider).Returns(data.Provider);
        mockDbSet.As<IQueryable<Domain.Entities.Impediment>>().Setup(m => m.Expression).Returns(data.Expression);
        mockDbSet.As<IQueryable<Domain.Entities.Impediment>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockDbSet.As<IQueryable<Domain.Entities.Impediment>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        mockDbSet.As<IAsyncEnumerable<Domain.Entities.Impediment>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<Domain.Entities.Impediment>(data.GetEnumerator()));

        mockDbContext.Setup(db => db.Impediments).Returns(mockDbSet.Object);

        return mockDbContext;
    }

    private class TestAsyncEnumerator<T>(IEnumerator<T> inner) : IAsyncEnumerator<T>
    {
        public ValueTask DisposeAsync()
        {
            inner.Dispose();
            return ValueTask.CompletedTask;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return ValueTask.FromResult(inner.MoveNext());
        }

        public T Current => inner.Current;
    }
}