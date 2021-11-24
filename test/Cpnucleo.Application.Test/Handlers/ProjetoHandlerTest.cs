using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Test.Handlers;

public class ProjetoHandlerTest
{
    [Fact]
    public async Task CreateProjetoCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();

        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));
        await unitOfWork.SaveChangesAsync();

        CreateProjetoCommand request = new()
        {
            Projeto = MockViewModelHelper.GetNewProjeto(sistemaId)
        };

        // Act
        ProjetoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetProjetoQuery_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();

        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();

        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));
        await unitOfWork.SaveChangesAsync();

        GetProjetoQuery request = new()
        {
            Id = projetoId
        };

        // Act
        ProjetoHandler handler = new(unitOfWork, mapper);
        ProjetoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Id != Guid.Empty);
        Assert.True(response.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListProjetoQuery_Handle()
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

        ListProjetoQuery request = new();

        // Act
        ProjetoHandler handler = new(unitOfWork, mapper);
        IEnumerable<ProjetoViewModel> response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Any());
        Assert.True(response.FirstOrDefault(x => x.Id == projetoId) != null);
    }

    [Fact]
    public async Task RemoveProjetoCommand_Handle()
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

        RemoveProjetoCommand request = new()
        {
            Id = projetoId
        };

        GetProjetoQuery request2 = new()
        {
            Id = projetoId
        };

        // Act
        ProjetoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        ProjetoViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 == null);
    }

    [Fact]
    public async Task UpdateProjetoCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();

        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();
        DateTime dataInclusao = DateTime.Now;

        Projeto projeto = MockEntityHelper.GetNewProjeto(sistemaId, projetoId);

        await unitOfWork.ProjetoRepository.AddAsync(projeto);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.ProjetoRepository.Detatch(projeto);

        UpdateProjetoCommand request = new()
        {
            Projeto = MockViewModelHelper.GetNewProjeto(sistemaId, projetoId, dataInclusao)
        };

        GetProjetoQuery request2 = new()
        {
            Id = projetoId
        };

        // Act
        ProjetoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        ProjetoViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 != null);
        Assert.True(response2.Id == projetoId);
        Assert.True(response2.DataInclusao.Ticks == dataInclusao.Ticks);
    }
}
