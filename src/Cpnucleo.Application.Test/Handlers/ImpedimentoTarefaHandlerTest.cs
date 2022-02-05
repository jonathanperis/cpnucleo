using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

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
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
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
        ImpedimentoTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Id != Guid.Empty);
        Assert.True(response.DataInclusao.Ticks != 0);
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
        IEnumerable<ImpedimentoTarefaViewModel> response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Any());
        Assert.True(response.FirstOrDefault(x => x.Id == impedimentoTarefaId) != null);
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
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        ImpedimentoTarefaViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 == null);
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
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        ImpedimentoTarefaViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 != null);
        Assert.True(response2.Id == impedimentoTarefaId);
        Assert.True(response2.DataInclusao.Ticks == dataInclusao.Ticks);
    }
}
