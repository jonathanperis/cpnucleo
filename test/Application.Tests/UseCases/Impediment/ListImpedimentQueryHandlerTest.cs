namespace Application.Tests.UseCases.Impediment;

public class ImpedimentQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnListOfImpediments_WhenImpedimentsExist()
    {
        // Arrange
        var mockDbContext = CreateMockDbContextWithData([
            Domain.Entities.Impediment.Create("Test Impediment 1", BaseEntity.GetNewId()),
            Domain.Entities.Impediment.Create("Test Impediment 2", BaseEntity.GetNewId())
        ]);

        var handler = new ListImpedimentQueryHandler(mockDbContext.Object);
        var request = new ListImpedimentQuery(new PaginationParams());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.Equal(2, result.Result.Data?.Count());
        Assert.Equal(2, result.Result.TotalCount);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoImpedimentsExist()
    {
        // Arrange
        var mockDbContext = CreateMockDbContextWithData(new List<Domain.Entities.Impediment>());

        var handler = new ListImpedimentQueryHandler(mockDbContext.Object);
        var request = new ListImpedimentQuery(new PaginationParams());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Empty(result.Result.Data);
        Assert.Equal(0, result.Result.TotalCount);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListImpedimentsReturnsNull()
    {
        // Arrange
        var mockDbContext = CreateMockDbContextWithData(new List<Domain.Entities.Impediment>());

        var handler = new ListImpedimentQueryHandler(mockDbContext.Object);
        var request = new ListImpedimentQuery(new PaginationParams());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Empty(result.Result.Data);
        Assert.Equal(0, result.Result.TotalCount);
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

        // Setup for EF Core async operations
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