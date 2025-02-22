namespace Application.Tests.UseCases.Impediment;

public class GetImpedimentByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnImpediment_WhenImpedimentExists()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var impediment = context.Impediments!.First();
        var impedimentId = impediment.Id;
        
        var request = new GetImpedimentByIdQuery(impedimentId);
        var handler = new GetImpedimentByIdQueryHandler(context);
        
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
        var context = DbContextHelper.GetContext();

        var request = new GetImpedimentByIdQuery(Guid.NewGuid());
        var handler = new GetImpedimentByIdQueryHandler(context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Impediment);
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