using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Test.Handlers;

public class ApontamentoHandlerTest
{
    [Fact]
    public async Task CreateApontamentoCommand_Handle()
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

        CreateApontamentoCommand request = new()
        {
            Apontamento = MockViewModelHelper.GetNewApontamento(tarefaId, recursoId)
        };

        // Act
        ApontamentoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetApontamentoQuery_Handle()
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

        GetApontamentoQuery request = new()
        {
            Id = apontamentoId
        };

        // Act
        ApontamentoHandler handler = new(unitOfWork, mapper);
        ApontamentoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Id != Guid.Empty);
        Assert.True(response.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListApontamentoQuery_Handle()
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

        ListApontamentoQuery request = new();

        // Act
        ApontamentoHandler handler = new(unitOfWork, mapper);
        IEnumerable<ApontamentoViewModel> response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Any());
        Assert.True(response.FirstOrDefault(x => x.Id == apontamentoId) != null);
    }

    [Fact]
    public async Task RemoveApontamentoCommand_Handle()
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

        RemoveApontamentoCommand request = new()
        {
            Id = apontamentoId
        };

        GetApontamentoQuery request2 = new()
        {
            Id = apontamentoId
        };

        // Act
        ApontamentoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        ApontamentoViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 == null);
    }

    [Fact]
    public async Task UpdateApontamentoCommand_Handle()
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
        DateTime dataInclusao = DateTime.Now;
        Apontamento apontamento = MockEntityHelper.GetNewApontamento(tarefaId, recursoId, apontamentoId);

        await unitOfWork.ApontamentoRepository.AddAsync(apontamento);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.ApontamentoRepository.Detatch(apontamento);

        UpdateApontamentoCommand request = new()
        {
            Apontamento = MockViewModelHelper.GetNewApontamento(tarefaId, recursoId, apontamentoId, dataInclusao)
        };

        GetApontamentoQuery request2 = new()
        {
            Id = apontamentoId
        };

        // Act
        ApontamentoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        ApontamentoViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 != null);
        Assert.True(response2.Id == apontamentoId);
        Assert.True(response2.DataInclusao.Ticks == dataInclusao.Ticks);
    }
}
