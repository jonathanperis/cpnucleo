using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.RemoveImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.UpdateImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.GetImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.ListImpedimentoTarefa;

namespace Cpnucleo.Application.Test.Handlers;

public class ImpedimentoTarefaHandlerTest
{
    [Fact]
    public async Task CreateImpedimentoTarefaCommand_Handle()
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

        CreateImpedimentoTarefaCommand request = new()
        {
            ImpedimentoTarefa = MockViewModelHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId)
        };

        // Act
        ImpedimentoTarefaHandler handler = new(unitOfWork, mapper);
        CreateImpedimentoTarefaResponse response = await handler.InvokeAsync(request, CancellationToken.None);

        // Assert
        Assert.True(response.Status == OperationResult.Success);
        Assert.True(response.ImpedimentoTarefa != null);
        Assert.True(response.ImpedimentoTarefa.Id != Guid.Empty);
        Assert.True(response.ImpedimentoTarefa.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task GetImpedimentoTarefaQuery_Handle()
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

        GetImpedimentoTarefaQuery request = new()
        {
            Id = impedimentoTarefaId
        };

        // Act
        ImpedimentoTarefaHandler handler = new(unitOfWork, mapper);
        GetImpedimentoTarefaResponse response = await handler.InvokeAsync(request, CancellationToken.None);

        // Assert
        Assert.True(response.Status == OperationResult.Success);
        Assert.True(response.ImpedimentoTarefa != null);
        Assert.True(response.ImpedimentoTarefa.Id != Guid.Empty);
        Assert.True(response.ImpedimentoTarefa.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListImpedimentoTarefaQuery_Handle()
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

        ListImpedimentoTarefaQuery request = new();

        // Act
        ImpedimentoTarefaHandler handler = new(unitOfWork, mapper);
        ListImpedimentoTarefaResponse response = await handler.InvokeAsync(request, CancellationToken.None);

        // Assert
        Assert.True(response.Status == OperationResult.Success);
        Assert.True(response.ImpedimentoTarefas != null);
        Assert.True(response.ImpedimentoTarefas.Any());
        Assert.True(response.ImpedimentoTarefas.FirstOrDefault(x => x.Id == impedimentoTarefaId) != null);
    }

    [Fact]
    public async Task RemoveImpedimentoTarefaCommand_Handle()
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

        RemoveImpedimentoTarefaCommand request = new()
        {
            Id = impedimentoTarefaId
        };

        GetImpedimentoTarefaQuery request2 = new()
        {
            Id = impedimentoTarefaId
        };

        // Act
        ImpedimentoTarefaHandler handler = new(unitOfWork, mapper);
        RemoveImpedimentoTarefaResponse response = await handler.InvokeAsync(request, CancellationToken.None);
        GetImpedimentoTarefaResponse response2 = await handler.InvokeAsync(request2, CancellationToken.None);

        // Assert
        Assert.True(response.Status == OperationResult.Success);
        Assert.True(response2.Status == OperationResult.NotFound);
        Assert.True(response2.ImpedimentoTarefa == null);
    }

    [Fact]
    public async Task UpdateImpedimentoTarefaCommand_Handle()
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
        DateTime dataInclusao = DateTime.Now;
        ImpedimentoTarefa impedimentoTarefa = MockEntityHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId, impedimentoTarefaId);

        await unitOfWork.ImpedimentoTarefaRepository.AddAsync(impedimentoTarefa);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.ImpedimentoTarefaRepository.Detatch(impedimentoTarefa);

        UpdateImpedimentoTarefaCommand request = new()
        {
            ImpedimentoTarefa = MockViewModelHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId, impedimentoTarefaId, dataInclusao)
        };

        GetImpedimentoTarefaQuery request2 = new()
        {
            Id = impedimentoTarefaId
        };

        // Act
        ImpedimentoTarefaHandler handler = new(unitOfWork, mapper);
        UpdateImpedimentoTarefaResponse response = await handler.InvokeAsync(request, CancellationToken.None);
        GetImpedimentoTarefaResponse response2 = await handler.InvokeAsync(request2, CancellationToken.None);

        // Assert
        Assert.True(response.Status == OperationResult.Success);
        Assert.True(response2.Status == OperationResult.Success);
        Assert.True(response2.ImpedimentoTarefa != null);
        Assert.True(response2.ImpedimentoTarefa.Id == impedimentoTarefaId);
        Assert.True(response2.ImpedimentoTarefa.DataInclusao.Ticks == dataInclusao.Ticks);
    }
}
