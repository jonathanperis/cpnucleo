namespace Cpnucleo.Application.Test.Handlers;

public class SistemaHandlerTest
{
    [Fact]
    public async Task CreateSistemaCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var request = MockCommandHelper.GetNewCreateSistemaCommand();

        // Act
        CreateSistemaCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetSistemaQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var sistema = context.Sistemas.First();

        var request = MockQueryHelper.GetNewGetSistemaQuery(sistema.Id);

        // Act
        GetSistemaQueryHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Sistema != null);
        Assert.True(response.Sistema.Id != Guid.Empty);
        Assert.True(response.Sistema.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListSistemaQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();
        IHubContext<ApplicationHub> mockSignalR = SignalRHelper.GetInstance();

        var request = MockQueryHelper.GetNewListSistemaQuery();

        // Act
        ListSistemaQueryHandler handler = new(context, mockSignalR);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Sistemas != null);
        Assert.True(response.Sistemas.Any());
    }

    [Fact]
    public async Task RemoveSistemaCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();
        
        var sistema = context.Sistemas.First();
        
        var eventManager = EventManagerHelper.GetInstance(new RemoveSistemaEvent(sistema.Id, sistema.Nome!));

        var request = MockCommandHelper.GetNewRemoveSistemaCommand(sistema.Id);
        var request2 = MockQueryHelper.GetNewGetSistemaQuery(sistema.Id);

        // Act
        RemoveSistemaCommandHandler handler = new(context, eventManager);
        var response = await handler.Handle(request, CancellationToken.None);

        GetSistemaQueryHandler handler2 = new(context);
        var response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateSistemaCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var sistema = context.Sistemas.First();

        var request = MockCommandHelper.GetNewUpdateSistemaCommand(sistema.Id);
        var request2 = MockQueryHelper.GetNewGetSistemaQuery(sistema.Id);

        // Act
        UpdateSistemaCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        GetSistemaQueryHandler handler2 = new(context);
        var response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Sistema != null);
        Assert.True(response2.Sistema.Id == sistema.Id);
    }
}