using Cpnucleo.Application.Commands.TipoTarefa;
using Cpnucleo.Application.Queries.TipoTarefa;
using Cpnucleo.Shared.Commands.TipoTarefa;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.TipoTarefa;

namespace Cpnucleo.Application.Test.Handlers;

public class TipoTarefaHandlerTest
{
    [Fact]
    public async Task CreateTipoTarefaCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        CreateTipoTarefaCommand request = MockCommandHelper.GetNewCreateTipoTarefaCommand();

        // Act
        CreateTipoTarefaHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetTipoTarefaQuery_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid tipoTarefaId = Guid.NewGuid();

        await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));
        await unitOfWork.SaveChangesAsync();

        GetTipoTarefaQuery request = MockQueryHelper.GetNewGetTipoTarefaQuery(tipoTarefaId);

        // Act
        GetTipoTarefaHandler handler = new(unitOfWork, mapper);
        GetTipoTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.TipoTarefa != null);
        Assert.True(response.TipoTarefa.Id != Guid.Empty);
        Assert.True(response.TipoTarefa.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListTipoTarefaQuery_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid tipoTarefaId = Guid.NewGuid();

        await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));
        await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa());
        await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa());

        await unitOfWork.SaveChangesAsync();

        ListTipoTarefaQuery request = MockQueryHelper.GetNewListTipoTarefaQuery();

        // Act
        ListTipoTarefaHandler handler = new(unitOfWork, mapper);
        ListTipoTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.TipoTarefas != null);
        Assert.True(response.TipoTarefas.Any());
        Assert.True(response.TipoTarefas.FirstOrDefault(x => x.Id == tipoTarefaId) != null);
    }

    [Fact]
    public async Task RemoveTipoTarefaCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid tipoTarefaId = Guid.NewGuid();

        TipoTarefa tipoTarefa = MockEntityHelper.GetNewTipoTarefa(tipoTarefaId);

        await unitOfWork.TipoTarefaRepository.AddAsync(tipoTarefa);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.TipoTarefaRepository.Detatch(tipoTarefa);

        RemoveTipoTarefaCommand request = MockCommandHelper.GetNewRemoveTipoTarefaCommand(tipoTarefaId);
        GetTipoTarefaQuery request2 = MockQueryHelper.GetNewGetTipoTarefaQuery(tipoTarefaId);

        // Act
        RemoveTipoTarefaHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetTipoTarefaHandler handler2 = new(unitOfWork, mapper);
        GetTipoTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateTipoTarefaCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid tipoTarefaId = Guid.NewGuid();

        TipoTarefa tipoTarefa = MockEntityHelper.GetNewTipoTarefa(tipoTarefaId);

        await unitOfWork.TipoTarefaRepository.AddAsync(tipoTarefa);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.TipoTarefaRepository.Detatch(tipoTarefa);

        UpdateTipoTarefaCommand request = MockCommandHelper.GetNewUpdateTipoTarefaCommand(tipoTarefaId);
        GetTipoTarefaQuery request2 = MockQueryHelper.GetNewGetTipoTarefaQuery(tipoTarefaId);

        // Act
        UpdateTipoTarefaHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetTipoTarefaHandler handler2 = new(unitOfWork, mapper);
        GetTipoTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.TipoTarefa != null);
        Assert.True(response2.TipoTarefa.Id == tipoTarefaId);
    }
}
