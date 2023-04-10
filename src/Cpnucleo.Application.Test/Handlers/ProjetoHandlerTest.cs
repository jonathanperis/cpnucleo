namespace Cpnucleo.Application.Test.Handlers;

public class ProjetoHandlerTest
{
    [Fact]
    public async Task CreateProjetoCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        await DbContextHelper.SeedData(context);

        var sistema = context.Sistemas.First();

        CreateProjetoCommand request = MockCommandHelper.GetNewCreateProjetoCommand(sistema.Id);

        // Act
        CreateProjetoCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetProjetoQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var projeto = context.Projetos.First();

        GetProjetoQuery request = MockQueryHelper.GetNewGetProjetoQuery(projeto.Id);

        // Act
        GetProjetoQueryHandler handler = new(context, mapper);
        GetProjetoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Projeto != null);
        Assert.True(response.Projeto.Id != Guid.Empty);
        Assert.True(response.Projeto.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListProjetoQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        ListProjetoQuery request = MockQueryHelper.GetNewListProjetoQuery();

        // Act
        ListProjetoQueryHandler handler = new(context, mapper);
        ListProjetoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Projetos != null);
        Assert.True(response.Projetos.Any());
    }

    [Fact]
    public async Task RemoveProjetoCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var projeto = context.Projetos.First();

        RemoveProjetoCommand request = MockCommandHelper.GetNewRemoveProjetoCommand(projeto.Id);
        GetProjetoQuery request2 = MockQueryHelper.GetNewGetProjetoQuery(projeto.Id);

        // Act
        RemoveProjetoCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetProjetoQueryHandler handler2 = new(context, mapper);
        GetProjetoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateProjetoCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var sistema = context.Sistemas.First();
        var projeto = context.Projetos.First();

        UpdateProjetoCommand request = MockCommandHelper.GetNewUpdateProjetoCommand(sistema.Id, projeto.Id);
        GetProjetoQuery request2 = MockQueryHelper.GetNewGetProjetoQuery(projeto.Id);

        // Act
        UpdateProjetoCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetProjetoQueryHandler handler2 = new(context, mapper);
        GetProjetoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Projeto != null);
        Assert.True(response2.Projeto.Id == projeto.Id);
    }
}
