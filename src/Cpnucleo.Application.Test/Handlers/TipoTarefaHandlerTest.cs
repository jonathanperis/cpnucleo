using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Test.Handlers;

public class TipoTarefaHandlerTest
{
    [Fact]
    public async Task CreateTipoTarefaCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        CreateTipoTarefaCommand request = new()
        {
            TipoTarefa = MockViewModelHelper.GetNewTipoTarefa()
        };

        // Act
        TipoTarefaHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetTipoTarefaQuery_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid tipoTarefaId = Guid.NewGuid();

        await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));
        await unitOfWork.SaveChangesAsync();

        GetTipoTarefaQuery request = new()
        {
            Id = tipoTarefaId
        };

        // Act
        TipoTarefaHandler handler = new(unitOfWork, mapper);
        TipoTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Id != Guid.Empty);
        Assert.True(response.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListTipoTarefaQuery_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid tipoTarefaId = Guid.NewGuid();

        await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));
        await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa());
        await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa());

        await unitOfWork.SaveChangesAsync();

        ListTipoTarefaQuery request = new();

        // Act
        TipoTarefaHandler handler = new(unitOfWork, mapper);
        IEnumerable<TipoTarefaViewModel> response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Any());
        Assert.True(response.FirstOrDefault(x => x.Id == tipoTarefaId) != null);
    }

    [Fact]
    public async Task RemoveTipoTarefaCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid tipoTarefaId = Guid.NewGuid();

        TipoTarefa tipoTarefa = MockEntityHelper.GetNewTipoTarefa(tipoTarefaId);

        await unitOfWork.TipoTarefaRepository.AddAsync(tipoTarefa);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.TipoTarefaRepository.Detatch(tipoTarefa);

        RemoveTipoTarefaCommand request = new()
        {
            Id = tipoTarefaId
        };

        GetTipoTarefaQuery request2 = new()
        {
            Id = tipoTarefaId
        };

        // Act
        TipoTarefaHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        TipoTarefaViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 == null);
    }

    [Fact]
    public async Task UpdateTipoTarefaCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid tipoTarefaId = Guid.NewGuid();
        DateTime dataInclusao = DateTime.Now;

        TipoTarefa tipoTarefa = MockEntityHelper.GetNewTipoTarefa(tipoTarefaId);

        await unitOfWork.TipoTarefaRepository.AddAsync(tipoTarefa);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.TipoTarefaRepository.Detatch(tipoTarefa);

        UpdateTipoTarefaCommand request = new()
        {
            TipoTarefa = MockViewModelHelper.GetNewTipoTarefa(tipoTarefaId, dataInclusao)
        };

        GetTipoTarefaQuery request2 = new()
        {
            Id = tipoTarefaId
        };

        // Act
        TipoTarefaHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        TipoTarefaViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 != null);
        Assert.True(response2.Id == tipoTarefaId);
        Assert.True(response2.DataInclusao.Ticks == dataInclusao.Ticks);
    }
}
