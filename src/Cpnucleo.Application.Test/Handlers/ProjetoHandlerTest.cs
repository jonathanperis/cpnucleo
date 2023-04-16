namespace Cpnucleo.Application.Test.Handlers;

public class ProjetoHandlerTest
{
    [Fact]
    public async Task CreateProjetoCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var sistema = context.Sistemas.First();

        var request = MockCommandHelper.GetNewCreateProjetoCommand(sistema.Id);

        // Act
        CreateProjetoCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetProjetoQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var projeto = context.Projetos.First();

        var request = MockQueryHelper.GetNewGetProjetoQuery(projeto.Id);

        // Act
        GetProjetoQueryHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Projeto != null);
        Assert.True(response.Projeto.Id != Guid.Empty);
        Assert.True(response.Projeto.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListProjetoQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var request = MockQueryHelper.GetNewListProjetoQuery();

        // Act
        ListProjetoQueryHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Projetos != null);
        Assert.True(response.Projetos.Any());
    }

    [Fact]
    public async Task RemoveProjetoCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var projeto = context.Projetos.First();

        var request = MockCommandHelper.GetNewRemoveProjetoCommand(projeto.Id);
        var request2 = MockQueryHelper.GetNewGetProjetoQuery(projeto.Id);

        // Act
        RemoveProjetoCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        GetProjetoQueryHandler handler2 = new(context);
        var response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateProjetoCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var sistema = context.Sistemas.First();
        var projeto = context.Projetos.First();

        var request = MockCommandHelper.GetNewUpdateProjetoCommand(sistema.Id, projeto.Id);
        var request2 = MockQueryHelper.GetNewGetProjetoQuery(projeto.Id);

        // Act
        UpdateProjetoCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        GetProjetoQueryHandler handler2 = new(context);
        var response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Projeto != null);
        Assert.True(response2.Projeto.Id == projeto.Id);
    }
}
