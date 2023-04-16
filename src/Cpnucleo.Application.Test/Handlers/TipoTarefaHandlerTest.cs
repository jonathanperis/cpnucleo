namespace Cpnucleo.Application.Test.Handlers;

public class TipoTarefaHandlerTest
{
    [Fact]
    public async Task CreateTipoTarefaCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var request = MockCommandHelper.GetNewCreateTipoTarefaCommand();

        // Act
        CreateTipoTarefaCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetTipoTarefaQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var tipoTarefa = context.TipoTarefas.First();

        var request = MockQueryHelper.GetNewGetTipoTarefaQuery(tipoTarefa.Id);

        // Act
        GetTipoTarefaQueryHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.TipoTarefa != null);
        Assert.True(response.TipoTarefa.Id != Guid.Empty);
        Assert.True(response.TipoTarefa.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListTipoTarefaQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var request = MockQueryHelper.GetNewListTipoTarefaQuery();

        // Act
        ListTipoTarefaQueryHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.TipoTarefas != null);
        Assert.True(response.TipoTarefas.Any());
    }

    [Fact]
    public async Task RemoveTipoTarefaCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var tipoTarefa = context.TipoTarefas.First();

        var request = MockCommandHelper.GetNewRemoveTipoTarefaCommand(tipoTarefa.Id);
        var request2 = MockQueryHelper.GetNewGetTipoTarefaQuery(tipoTarefa.Id);

        // Act
        RemoveTipoTarefaCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        GetTipoTarefaQueryHandler handler2 = new(context);
        var response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateTipoTarefaCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var tipoTarefa = context.TipoTarefas.First();

        var request = MockCommandHelper.GetNewUpdateTipoTarefaCommand(tipoTarefa.Id);
        var request2 = MockQueryHelper.GetNewGetTipoTarefaQuery(tipoTarefa.Id);

        // Act
        UpdateTipoTarefaCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        GetTipoTarefaQueryHandler handler2 = new(context);
        var response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.TipoTarefa != null);
        Assert.True(response2.TipoTarefa.Id == tipoTarefa.Id);
    }
}
