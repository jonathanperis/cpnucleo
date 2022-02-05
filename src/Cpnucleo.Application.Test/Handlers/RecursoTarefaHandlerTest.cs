using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Test.Handlers;

public class RecursoTarefaHandlerTest
{
    [Fact]
    public async Task CreateRecursoTarefaCommand_Handle()
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

        CreateRecursoTarefaCommand request = new()
        {
            RecursoTarefa = MockViewModelHelper.GetNewRecursoTarefa(tarefaId, recursoId)
        };

        // Act
        RecursoTarefaHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetRecursoTarefaQuery_Handle()
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

        Guid recursoTarefaId = Guid.NewGuid();
        await unitOfWork.RecursoTarefaRepository.AddAsync(MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId, recursoTarefaId));

        await unitOfWork.SaveChangesAsync();

        GetRecursoTarefaQuery request = new()
        {
            Id = recursoTarefaId
        };

        // Act
        RecursoTarefaHandler handler = new(unitOfWork, mapper);
        RecursoTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Id != Guid.Empty);
        Assert.True(response.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListRecursoTarefaQuery_Handle()
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

        Guid recursoTarefaId = Guid.NewGuid();
        await unitOfWork.RecursoTarefaRepository.AddAsync(MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId, recursoTarefaId));
        await unitOfWork.RecursoTarefaRepository.AddAsync(MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId));
        await unitOfWork.RecursoTarefaRepository.AddAsync(MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId));

        await unitOfWork.SaveChangesAsync();

        ListRecursoTarefaQuery request = new();

        // Act
        RecursoTarefaHandler handler = new(unitOfWork, mapper);
        IEnumerable<RecursoTarefaViewModel> response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Any());
        Assert.True(response.FirstOrDefault(x => x.Id == recursoTarefaId) != null);
    }

    [Fact]
    public async Task RemoveRecursoTarefaCommand_Handle()
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

        Guid recursoTarefaId = Guid.NewGuid();
        RecursoTarefa recursoTarefa = MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId, recursoTarefaId);

        await unitOfWork.RecursoTarefaRepository.AddAsync(recursoTarefa);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.RecursoTarefaRepository.Detatch(recursoTarefa);

        RemoveRecursoTarefaCommand request = new()
        {
            Id = recursoTarefaId
        };

        GetRecursoTarefaQuery request2 = new()
        {
            Id = recursoTarefaId
        };

        // Act
        RecursoTarefaHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        RecursoTarefaViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 == null);
    }

    [Fact]
    public async Task UpdateRecursoTarefaCommand_Handle()
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

        Guid recursoTarefaId = Guid.NewGuid();
        DateTime dataInclusao = DateTime.Now;
        RecursoTarefa recursoTarefa = MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId, recursoTarefaId);

        await unitOfWork.RecursoTarefaRepository.AddAsync(recursoTarefa);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.RecursoTarefaRepository.Detatch(recursoTarefa);

        UpdateRecursoTarefaCommand request = new()
        {
            RecursoTarefa = MockViewModelHelper.GetNewRecursoTarefa(tarefaId, recursoId, recursoTarefaId, dataInclusao)
        };

        GetRecursoTarefaQuery request2 = new()
        {
            Id = recursoTarefaId
        };

        // Act
        RecursoTarefaHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        RecursoTarefaViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 != null);
        Assert.True(response2.Id == recursoTarefaId);
        Assert.True(response2.DataInclusao.Ticks == dataInclusao.Ticks);
    }
}
