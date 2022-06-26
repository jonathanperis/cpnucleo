using Cpnucleo.Application.Commands.ImpedimentoTarefa;
using Cpnucleo.Application.Queries.ImpedimentoTarefa;

namespace Cpnucleo.Application.Test.Handlers;

public class ImpedimentoTarefaHandlerTest
{
    [Fact]
    public async Task CreateImpedimentoTarefaCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();
        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();
        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

        Guid workflowId = Guid.NewGuid();
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

        Guid recursoId = Guid.NewGuid();
        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

        Guid tipoTarefaId = Guid.NewGuid();
        await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));

        Guid tarefaId = Guid.NewGuid();
        await unitOfWork.TarefaRepository.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId));

        Guid impedimentoId = Guid.NewGuid();
        await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento(impedimentoId));

        await unitOfWork.SaveChangesAsync();

        CreateImpedimentoTarefaCommand request = MockCommandHelper.GetNewCreateImpedimentoTarefaCommand(tarefaId, impedimentoId);

        // Act
        CreateImpedimentoTarefaHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetImpedimentoTarefaQuery_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();
        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();
        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

        Guid workflowId = Guid.NewGuid();
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

        Guid recursoId = Guid.NewGuid();
        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

        Guid tipoTarefaId = Guid.NewGuid();
        await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));

        Guid tarefaId = Guid.NewGuid();
        await unitOfWork.TarefaRepository.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId));

        Guid impedimentoId = Guid.NewGuid();
        await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento(impedimentoId));

        Guid impedimentoTarefaId = Guid.NewGuid();
        await unitOfWork.ImpedimentoTarefaRepository.AddAsync(MockEntityHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId, impedimentoTarefaId));

        await unitOfWork.SaveChangesAsync();

        GetImpedimentoTarefaQuery request = MockQueryHelper.GetNewGetImpedimentoTarefaQuery(impedimentoTarefaId);

        // Act
        GetImpedimentoTarefaHandler handler = new(unitOfWork, mapper);
        GetImpedimentoTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.ImpedimentoTarefa != null);
        Assert.True(response.ImpedimentoTarefa.Id != Guid.Empty);
        Assert.True(response.ImpedimentoTarefa.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task GetImpedimentoTarefaByTarefaQuery_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();
        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();
        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

        Guid workflowId = Guid.NewGuid();
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

        Guid recursoId = Guid.NewGuid();
        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

        Guid tipoTarefaId = Guid.NewGuid();
        await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));

        Guid tarefaId = Guid.NewGuid();
        await unitOfWork.TarefaRepository.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId));

        Guid impedimentoId = Guid.NewGuid();
        await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento(impedimentoId));

        Guid impedimentoTarefaId = Guid.NewGuid();
        await unitOfWork.ImpedimentoTarefaRepository.AddAsync(MockEntityHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId, impedimentoTarefaId));

        await unitOfWork.SaveChangesAsync();

        GetImpedimentoTarefaByTarefaQuery request = MockQueryHelper.GetNewGetImpedimentoTarefaByTarefaQuery(tarefaId);

        // Act
        GetImpedimentoTarefaByTarefaHandler handler = new(unitOfWork, mapper);
        GetImpedimentoTarefaByTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.ImpedimentoTarefas != null);
        Assert.True(response.ImpedimentoTarefas.Any());
        Assert.True(response.ImpedimentoTarefas.FirstOrDefault(x => x.Id == impedimentoTarefaId) != null);
    }

    [Fact]
    public async Task ListImpedimentoTarefaQuery_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();
        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();
        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

        Guid workflowId = Guid.NewGuid();
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

        Guid recursoId = Guid.NewGuid();
        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

        Guid tipoTarefaId = Guid.NewGuid();
        await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));

        Guid tarefaId = Guid.NewGuid();
        await unitOfWork.TarefaRepository.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId));

        Guid impedimentoId = Guid.NewGuid();
        await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento(impedimentoId));

        Guid impedimentoTarefaId = Guid.NewGuid();
        await unitOfWork.ImpedimentoTarefaRepository.AddAsync(MockEntityHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId, impedimentoTarefaId));
        await unitOfWork.ImpedimentoTarefaRepository.AddAsync(MockEntityHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId));
        await unitOfWork.ImpedimentoTarefaRepository.AddAsync(MockEntityHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId));

        await unitOfWork.SaveChangesAsync();

        ListImpedimentoTarefaQuery request = MockQueryHelper.GetNewListImpedimentoTarefaQuery();

        // Act
        ListImpedimentoTarefaHandler handler = new(unitOfWork, mapper);
        ListImpedimentoTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.ImpedimentoTarefas != null);
        Assert.True(response.ImpedimentoTarefas.Any());
        Assert.True(response.ImpedimentoTarefas.FirstOrDefault(x => x.Id == impedimentoTarefaId) != null);
    }

    [Fact]
    public async Task RemoveImpedimentoTarefaCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();
        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();
        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

        Guid workflowId = Guid.NewGuid();
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

        Guid recursoId = Guid.NewGuid();
        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

        Guid tipoTarefaId = Guid.NewGuid();
        await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));

        Guid tarefaId = Guid.NewGuid();
        await unitOfWork.TarefaRepository.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId));

        Guid impedimentoId = Guid.NewGuid();
        await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento(impedimentoId));

        Guid impedimentoTarefaId = Guid.NewGuid();
        ImpedimentoTarefa impedimentoTarefa = MockEntityHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId, impedimentoTarefaId);

        await unitOfWork.ImpedimentoTarefaRepository.AddAsync(impedimentoTarefa);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.ImpedimentoTarefaRepository.Detatch(impedimentoTarefa);

        RemoveImpedimentoTarefaCommand request = MockCommandHelper.GetNewRemoveImpedimentoTarefaCommand(impedimentoTarefaId);
        GetImpedimentoTarefaQuery request2 = MockQueryHelper.GetNewGetImpedimentoTarefaQuery(impedimentoTarefaId);

        // Act
        RemoveImpedimentoTarefaHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetImpedimentoTarefaHandler handler2 = new(unitOfWork, mapper);
        GetImpedimentoTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateImpedimentoTarefaCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();
        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();
        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

        Guid workflowId = Guid.NewGuid();
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

        Guid recursoId = Guid.NewGuid();
        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

        Guid tipoTarefaId = Guid.NewGuid();
        await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));

        Guid tarefaId = Guid.NewGuid();
        await unitOfWork.TarefaRepository.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId));

        Guid impedimentoId = Guid.NewGuid();
        await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento(impedimentoId));

        Guid impedimentoTarefaId = Guid.NewGuid();
        ImpedimentoTarefa impedimentoTarefa = MockEntityHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId, impedimentoTarefaId);

        await unitOfWork.ImpedimentoTarefaRepository.AddAsync(impedimentoTarefa);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.ImpedimentoTarefaRepository.Detatch(impedimentoTarefa);

        UpdateImpedimentoTarefaCommand request = MockCommandHelper.GetNewUpdateImpedimentoTarefaCommand(tarefaId, impedimentoId, impedimentoTarefaId);
        GetImpedimentoTarefaQuery request2 = MockQueryHelper.GetNewGetImpedimentoTarefaQuery(impedimentoTarefaId);

        // Act
        UpdateImpedimentoTarefaHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetImpedimentoTarefaHandler handler2 = new(unitOfWork, mapper);
        GetImpedimentoTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.ImpedimentoTarefa != null);
        Assert.True(response2.ImpedimentoTarefa.Id == impedimentoTarefaId);
    }
}
