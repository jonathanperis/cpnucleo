namespace Application.Tests.UseCases.Impediment;

public class ListImpedimentQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnListOfImpediments_WhenImpedimentsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };
        var context = DbContextHelper.GetContext();

        var request = new ListImpedimentQuery(pagination);
        var handler = new ListImpedimentQueryHandler(context);
        
        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.Equal(3, result.Result?.Data?.Count());
        Assert.Equal(3, result.Result?.TotalCount);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoImpedimentsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };
        var context = DbContextHelper.GetContext();

        context.Impediments?.RemoveRange(context.Impediments);
        await context.SaveChangesAsync();
        
        var request = new ListImpedimentQuery(pagination);
        var handler = new ListImpedimentQueryHandler(context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.NotNull(result.Result?.Data);
        Assert.Empty(result.Result.Data);
    }
}