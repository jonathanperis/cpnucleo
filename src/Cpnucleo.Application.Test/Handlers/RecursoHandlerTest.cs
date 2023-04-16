namespace Cpnucleo.Application.Test.Handlers;

public class RecursoHandlerTest
{
    [Fact]
    public async Task CreateRecursoCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var request = MockCommandHelper.GetNewCreateRecursoCommand();

        // Act
        CreateRecursoCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetRecursoQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var recurso = context.Recursos.First();

        var request = MockQueryHelper.GetNewGetRecursoQuery(recurso.Id);

        // Act
        GetRecursoQueryHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Recurso != null);
        Assert.True(response.Recurso.Id != Guid.Empty);
        Assert.True(response.Recurso.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListRecursoQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var request = MockQueryHelper.GetNewListRecursoQuery();

        // Act
        ListRecursoQueryHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Recursos != null);
        Assert.True(response.Recursos.Any());
    }

    [Fact]
    public async Task RemoveRecursoCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var recurso = context.Recursos.First();

        var request = MockCommandHelper.GetNewRemoveRecursoCommand(recurso.Id);
        var request2 = MockQueryHelper.GetNewGetRecursoQuery(recurso.Id);

        // Act
        RemoveRecursoCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        GetRecursoQueryHandler handler2 = new(context);
        var response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateRecursoCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var recurso = context.Recursos.First();

        var request = MockCommandHelper.GetNewUpdateRecursoCommand(recurso.Id);
        var request2 = MockQueryHelper.GetNewGetRecursoQuery(recurso.Id);

        // Act
        UpdateRecursoCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        GetRecursoQueryHandler handler2 = new(context);
        var response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Recurso != null);
        Assert.True(response2.Recurso.Id == recurso.Id);
    }
}
