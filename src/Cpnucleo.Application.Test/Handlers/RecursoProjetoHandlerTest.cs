namespace Cpnucleo.Application.Test.Handlers;

public class RecursoProjetoHandlerTest
{
    [Fact]
    public async Task CreateRecursoProjetoCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var projeto = context.Projetos.First();
        var recurso = context.Recursos.First();

        var request = MockCommandHelper.GetNewCreateRecursoProjetoCommand(projeto.Id, recurso.Id);

        // Act
        CreateRecursoProjetoCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetRecursoProjetoQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var recursoProjeto = context.RecursoProjetos.First();

        var request = MockQueryHelper.GetNewGetRecursoProjetoQuery(recursoProjeto.Id);

        // Act
        GetRecursoProjetoQueryHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.RecursoProjeto != null);
        Assert.True(response.RecursoProjeto.Id != Guid.Empty);
        Assert.True(response.RecursoProjeto.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListRecursoProjetoQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var request = MockQueryHelper.GetNewListRecursoProjetoQuery();

        // Act
        ListRecursoProjetoQueryHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.RecursoProjetos != null);
        Assert.True(response.RecursoProjetos.Any());
    }

    [Fact]
    public async Task RemoveRecursoProjetoCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var recursoProjeto = context.RecursoProjetos.First();

        var request = MockCommandHelper.GetNewRemoveRecursoProjetoCommand(recursoProjeto.Id);
        var request2 = MockQueryHelper.GetNewGetRecursoProjetoQuery(recursoProjeto.Id);

        // Act
        RemoveRecursoProjetoCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        GetRecursoProjetoQueryHandler handler2 = new(context);
        var response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateRecursoProjetoCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var projeto = context.Projetos.First();
        var recurso = context.Recursos.First();
        var recursoProjeto = context.RecursoProjetos.First();

        var request = MockCommandHelper.GetNewUpdateRecursoProjetoCommand(projeto.Id, recurso.Id, recursoProjeto.Id);
        var request2 = MockQueryHelper.GetNewGetRecursoProjetoQuery(recursoProjeto.Id);

        // Act
        UpdateRecursoProjetoCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        GetRecursoProjetoQueryHandler handler2 = new(context);
        var response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.RecursoProjeto != null);
        Assert.True(response2.RecursoProjeto.Id == recursoProjeto.Id);
    }

    [Fact]
    public async Task ListRecursoProjetoByProjetoQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var projeto = context.Projetos.First();

        var request = MockQueryHelper.GetNewListRecursoProjetoByProjetoQuery(projeto.Id);

        // Act
        ListRecursoProjetoByProjetoQueryHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.RecursoProjetos != null);
        Assert.True(response.RecursoProjetos.Any());
    }
}
