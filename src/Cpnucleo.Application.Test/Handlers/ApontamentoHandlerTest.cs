using Cpnucleo.Application.Commands.Apontamento;
using Cpnucleo.Application.Queries.Apontamento;

namespace Cpnucleo.Application.Test.Handlers;

public class ApontamentoHandlerTest
{
    [Fact]
    public async Task CreateApontamentoCommand_Handle_Success()
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

        await unitOfWork.SaveChangesAsync();

        CreateApontamentoCommand request = MockCommandHelper.GetNewCreateApontamentoCommand(tarefaId, recursoId);

        // Act
        CreateApontamentoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetApontamentoQuery_Handle_Success()
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

        Guid apontamentoId = Guid.NewGuid();
        await unitOfWork.ApontamentoRepository.AddAsync(MockEntityHelper.GetNewApontamento(tarefaId, recursoId, apontamentoId));

        await unitOfWork.SaveChangesAsync();

        GetApontamentoQuery request = MockQueryHelper.GetNewGetApontamentoQuery(apontamentoId);

        // Act
        GetApontamentoHandler handler = new(unitOfWork, mapper);
        GetApontamentoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Apontamento != null);
        Assert.True(response.Apontamento.Id != Guid.Empty);
        Assert.True(response.Apontamento.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task GetApontamentoByRecursoQuery_Handle_Success()
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

        Guid apontamentoId = Guid.NewGuid();
        await unitOfWork.ApontamentoRepository.AddAsync(MockEntityHelper.GetNewApontamento(tarefaId, recursoId, apontamentoId));

        await unitOfWork.SaveChangesAsync();

        ListApontamentoByRecursoQuery request = MockQueryHelper.GetApontamentoByRecursoQuery(recursoId);

        // Act
        GetApontamentoByRecursoHandler handler = new(unitOfWork, mapper);
        ListApontamentoByRecursoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Apontamentos != null);
        Assert.True(response.Apontamentos.Any());
        Assert.True(response.Apontamentos.FirstOrDefault(x => x.Id == apontamentoId) != null);
    }

    [Fact]
    public async Task ListApontamentoQuery_Handle_Success()
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

        Guid apontamentoId = Guid.NewGuid();
        await unitOfWork.ApontamentoRepository.AddAsync(MockEntityHelper.GetNewApontamento(tarefaId, recursoId, apontamentoId));
        await unitOfWork.ApontamentoRepository.AddAsync(MockEntityHelper.GetNewApontamento(tarefaId, recursoId));
        await unitOfWork.ApontamentoRepository.AddAsync(MockEntityHelper.GetNewApontamento(tarefaId, recursoId));

        await unitOfWork.SaveChangesAsync();

        ListApontamentoQuery request = MockQueryHelper.GetNewListApontamentoQuery();

        // Act
        ListApontamentoHandler handler = new(unitOfWork, mapper);
        ListApontamentoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Apontamentos != null);
        Assert.True(response.Apontamentos.Any());
        Assert.True(response.Apontamentos.FirstOrDefault(x => x.Id == apontamentoId) != null);
    }

    [Fact]
    public async Task RemoveApontamentoCommand_Handle_Success()
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

        Guid apontamentoId = Guid.NewGuid();
        Apontamento apontamento = MockEntityHelper.GetNewApontamento(tarefaId, recursoId, apontamentoId);

        await unitOfWork.ApontamentoRepository.AddAsync(apontamento);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.ApontamentoRepository.Detatch(apontamento);

        RemoveApontamentoCommand request = MockCommandHelper.GetNewRemoveApontamentoCommand(apontamentoId);
        GetApontamentoQuery request2 = MockQueryHelper.GetNewGetApontamentoQuery(apontamentoId);

        // Act
        RemoveApontamentoHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetApontamentoHandler handler2 = new(unitOfWork, mapper);
        GetApontamentoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateApontamentoCommand_Handle_Success()
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

        Guid apontamentoId = Guid.NewGuid();
        Apontamento apontamento = MockEntityHelper.GetNewApontamento(tarefaId, recursoId, apontamentoId);

        await unitOfWork.ApontamentoRepository.AddAsync(apontamento);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.ApontamentoRepository.Detatch(apontamento);

        UpdateApontamentoCommand request = MockCommandHelper.GetNewUpdateApontamentoCommand(tarefaId, recursoId, apontamentoId);
        GetApontamentoQuery request2 = MockQueryHelper.GetNewGetApontamentoQuery(apontamentoId);

        // Act
        UpdateApontamentoHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetApontamentoHandler handler2 = new(unitOfWork, mapper);
        GetApontamentoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Apontamento != null);
        Assert.True(response2.Apontamento.Id == apontamentoId);
    }
}
