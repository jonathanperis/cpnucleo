using Cpnucleo.Application.Commands.Projeto;
using Cpnucleo.Application.Queries.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;

namespace Cpnucleo.Application.Test.Handlers;

public class ProjetoHandlerTest
{
    [Fact]
    public async Task CreateProjetoCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();

        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));
        await unitOfWork.SaveChangesAsync();

        CreateProjetoCommand request = MockCommandHelper.GetNewCreateProjetoCommand(sistemaId);

        // Act
        CreateProjetoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetProjetoQuery_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();

        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();

        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));
        await unitOfWork.SaveChangesAsync();

        GetProjetoQuery request = MockQueryHelper.GetNewGetProjetoQuery(projetoId);

        // Act
        GetProjetoHandler handler = new(unitOfWork, mapper);
        GetProjetoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Projeto.Id != Guid.Empty);
        Assert.True(response.Projeto.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListProjetoQuery_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();

        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();

        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));
        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId));
        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId));

        await unitOfWork.SaveChangesAsync();

        ListProjetoQuery request = MockQueryHelper.GetNewListProjetoQuery();

        // Act
        ListProjetoHandler handler = new(unitOfWork, mapper);
        ListProjetoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Projetos.Any());
        Assert.True(response.Projetos.FirstOrDefault(x => x.Id == projetoId) != null);
    }

    [Fact]
    public async Task RemoveProjetoCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();

        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();

        Projeto projeto = MockEntityHelper.GetNewProjeto(sistemaId, projetoId);

        await unitOfWork.ProjetoRepository.AddAsync(projeto);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.ProjetoRepository.Detatch(projeto);

        RemoveProjetoCommand request = MockCommandHelper.GetNewRemoveProjetoCommand(projetoId);
        GetProjetoQuery request2 = MockQueryHelper.GetNewGetProjetoQuery(projetoId);

        // Act
        RemoveProjetoHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetProjetoHandler handler2 = new(unitOfWork, mapper);
        GetProjetoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateProjetoCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();

        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();

        Projeto projeto = MockEntityHelper.GetNewProjeto(sistemaId, projetoId);

        await unitOfWork.ProjetoRepository.AddAsync(projeto);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.ProjetoRepository.Detatch(projeto);

        UpdateProjetoCommand request = MockCommandHelper.GetNewUpdateProjetoCommand(sistemaId, projetoId);
        GetProjetoQuery request2 = MockQueryHelper.GetNewGetProjetoQuery(projetoId);

        // Act
        UpdateProjetoHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetProjetoHandler handler2 = new(unitOfWork, mapper);
        GetProjetoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 != null);
        Assert.True(response2.Projeto.Id == projetoId);
    }
}
