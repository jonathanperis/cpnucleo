namespace Cpnucleo.Application.Test.Handlers;

public class RecursoProjetoHandlerTest
{
    [Fact]
    public async Task CreateRecursoProjetoCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        await DbContextHelper.SeedData(context);

        var projeto = context.Projetos.First();
        var recurso = context.Recursos.First();

        CreateRecursoProjetoCommand request = MockCommandHelper.GetNewCreateRecursoProjetoCommand(projeto.Id, recurso.Id);

        // Act
        CreateRecursoProjetoCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetRecursoProjetoQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var recursoProjeto = context.RecursoProjetos.First();

        GetRecursoProjetoQuery request = MockQueryHelper.GetNewGetRecursoProjetoQuery(recursoProjeto.Id);

        // Act
        GetRecursoProjetoQueryHandler handler = new(context, mapper);
        GetRecursoProjetoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.RecursoProjeto != null);
        Assert.True(response.RecursoProjeto.Id != Guid.Empty);
        Assert.True(response.RecursoProjeto.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListRecursoProjetoQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        ListRecursoProjetoQuery request = MockQueryHelper.GetNewListRecursoProjetoQuery();

        // Act
        ListRecursoProjetoQueryHandler handler = new(context, mapper);
        ListRecursoProjetoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.RecursoProjetos != null);
        Assert.True(response.RecursoProjetos.Any());
    }

    [Fact]
    public async Task RemoveRecursoProjetoCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var recursoProjeto = context.RecursoProjetos.First();

        RemoveRecursoProjetoCommand request = MockCommandHelper.GetNewRemoveRecursoProjetoCommand(recursoProjeto.Id);
        GetRecursoProjetoQuery request2 = MockQueryHelper.GetNewGetRecursoProjetoQuery(recursoProjeto.Id);

        // Act
        RemoveRecursoProjetoCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetRecursoProjetoQueryHandler handler2 = new(context, mapper);
        GetRecursoProjetoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateRecursoProjetoCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var projeto = context.Projetos.First();
        var recurso = context.Recursos.First();
        var recursoProjeto = context.RecursoProjetos.First();

        UpdateRecursoProjetoCommand request = MockCommandHelper.GetNewUpdateRecursoProjetoCommand(projeto.Id, recurso.Id, recursoProjeto.Id);
        GetRecursoProjetoQuery request2 = MockQueryHelper.GetNewGetRecursoProjetoQuery(recursoProjeto.Id);

        // Act
        UpdateRecursoProjetoCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetRecursoProjetoQueryHandler handler2 = new(context, mapper);
        GetRecursoProjetoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.RecursoProjeto != null);
        Assert.True(response2.RecursoProjeto.Id == recursoProjeto.Id);
    }
}
