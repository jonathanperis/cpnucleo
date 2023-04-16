namespace Cpnucleo.Application.Test.Handlers;

public class RecursoTarefaHandlerTest
{
    [Fact]
    public async Task CreateRecursoTarefaCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var tarefa = context.Tarefas.First();
        var recurso = context.Recursos.First();

        var request = MockCommandHelper.GetNewCreateRecursoTarefaCommand(tarefa.Id, recurso.Id);

        // Act
        CreateRecursoTarefaCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetRecursoTarefaQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var recursoTarefa = context.RecursoTarefas.First();

        var request = MockQueryHelper.GetNewGetRecursoTarefaQuery(recursoTarefa.Id);

        // Act
        GetRecursoTarefaQueryHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.RecursoTarefa != null);
        Assert.True(response.RecursoTarefa.Id != Guid.Empty);
        Assert.True(response.RecursoTarefa.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListRecursoTarefaQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var request = MockQueryHelper.GetNewListRecursoTarefaQuery();

        // Act
        ListRecursoTarefaQueryHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.RecursoTarefas != null);
        Assert.True(response.RecursoTarefas.Any());
    }

    [Fact]
    public async Task RemoveRecursoTarefaCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var recursoTarefa = context.RecursoTarefas.First();

        var request = MockCommandHelper.GetNewRemoveRecursoTarefaCommand(recursoTarefa.Id);
        var request2 = MockQueryHelper.GetNewGetRecursoTarefaQuery(recursoTarefa.Id);

        // Act
        RemoveRecursoTarefaCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        GetRecursoTarefaQueryHandler handler2 = new(context);
        var response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateRecursoTarefaCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var tarefa = context.Tarefas.First();
        var recurso = context.Recursos.First();
        var recursoTarefa = context.RecursoTarefas.First();

        var request = MockCommandHelper.GetNewUpdateRecursoTarefaCommand(tarefa.Id, recurso.Id, recursoTarefa.Id);
        var request2 = MockQueryHelper.GetNewGetRecursoTarefaQuery(recursoTarefa.Id);

        // Act
        UpdateRecursoTarefaCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        GetRecursoTarefaQueryHandler handler2 = new(context);
        var response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.RecursoTarefa != null);
        Assert.True(response2.RecursoTarefa.Id == recursoTarefa.Id);
    }

    [Fact]
    public async Task ListRecursoTarefaByTarefaQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var tarefa = context.Tarefas.First();

        var request = MockQueryHelper.GetNewListRecursoTarefaByTarefaQuery(tarefa.Id);

        // Act
        ListRecursoTarefaByTarefaQueryHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.RecursoTarefas != null);
        Assert.True(response.RecursoTarefas.Any());
    }
}
