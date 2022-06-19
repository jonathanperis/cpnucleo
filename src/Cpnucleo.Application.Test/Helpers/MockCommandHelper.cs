namespace Cpnucleo.Application.Test.Helpers;

public class MockCommandHelper
{
    public static CreateSistemaCommand GetNewCreateSistemaCommand(Guid sistemaId = default)
    {
        return new CreateSistemaCommand(sistemaId == Guid.Empty ? Guid.NewGuid() : sistemaId, "Sistema de teste", "Descrição do sistema de teste");
    }

    public static RemoveSistemaCommand GetNewRemoveSistemaCommand(Guid sistemaId = default)
    {
        return new RemoveSistemaCommand(sistemaId == Guid.Empty ? Guid.NewGuid() : sistemaId);
    }

    public static UpdateSistemaCommand GetNewUpdateSistemaCommand(Guid sistemaId = default)
    {
        return new UpdateSistemaCommand(sistemaId == Guid.Empty ? Guid.NewGuid() : sistemaId, "Sistema de teste - alterado", "Descrição do sistema de teste - alterado");
    }

    public static CreateProjetoCommand GetNewCreateProjetoCommand(Guid sistemaId, Guid projetoId = default)
    {
        return new CreateProjetoCommand(projetoId == Guid.Empty ? Guid.NewGuid() : projetoId, "Projeto de teste", sistemaId);
    }

    public static RemoveProjetoCommand GetNewRemoveProjetoCommand(Guid projetoId = default)
    {
        return new RemoveProjetoCommand(projetoId == Guid.Empty ? Guid.NewGuid() : projetoId);
    }

    public static UpdateProjetoCommand GetNewUpdateProjetoCommand(Guid sistemaId, Guid projetoId = default)
    {
        return new UpdateProjetoCommand(projetoId == Guid.Empty ? Guid.NewGuid() : projetoId, "Projeto de teste - alterado", sistemaId);
    }

    public static CreateWorkflowCommand GetNewCreateWorkflowCommand(Guid workflowId = default)
    {
        return new CreateWorkflowCommand(workflowId == Guid.Empty ? Guid.NewGuid() : workflowId, "Workflow de teste", 1);
    }

    public static RemoveWorkflowCommand GetNewRemoveWorkflowCommand(Guid workflowId = default)
    {
        return new RemoveWorkflowCommand(workflowId == Guid.Empty ? Guid.NewGuid() : workflowId);
    }

    public static UpdateWorkflowCommand GetNewUpdateWorkflowCommand(Guid workflowId = default)
    {
        return new UpdateWorkflowCommand(workflowId == Guid.Empty ? Guid.NewGuid() : workflowId, "Workflow de teste - alterado", 2);
    }

    public static CreateImpedimentoCommand GetNewCreateImpedimentoCommand(Guid impedimentoId = default)
    {
        return new CreateImpedimentoCommand(impedimentoId == Guid.Empty ? Guid.NewGuid() : impedimentoId, "Impedimento de teste");
    }

    public static RemoveImpedimentoCommand GetNewRemoveImpedimentoCommand(Guid impedimentoId = default)
    {
        return new RemoveImpedimentoCommand(impedimentoId == Guid.Empty ? Guid.NewGuid() : impedimentoId);
    }

    public static UpdateImpedimentoCommand GetNewUpdateImpedimentoCommand(Guid impedimentoId = default)
    {
        return new UpdateImpedimentoCommand(impedimentoId == Guid.Empty ? Guid.NewGuid() : impedimentoId, "Impedimento de teste - alterado");
    }

    public static CreateRecursoCommand GetNewCreateRecursoCommand(Guid recursoId = default)
    {
        return new CreateRecursoCommand(recursoId == Guid.Empty ? Guid.NewGuid() : recursoId, "Recurso de teste", "usuario.teste", "12345678");
    }

    public static RemoveRecursoCommand GetNewRemoveRecursoCommand(Guid recursoId = default)
    {
        return new RemoveRecursoCommand(recursoId == Guid.Empty ? Guid.NewGuid() : recursoId);
    }

    public static UpdateRecursoCommand GetNewUpdateRecursoCommand(Guid recursoId = default)
    {
        return new UpdateRecursoCommand(recursoId == Guid.Empty ? Guid.NewGuid() : recursoId, "Recurso de teste - alterado", "1234567890");
    }

    public static CreateApontamentoCommand GetNewCreateApontamentoCommand(Guid tarefaId, Guid recursoId, Guid apontamentoId = default)
    {
        return new CreateApontamentoCommand(apontamentoId == Guid.Empty ? Guid.NewGuid() : apontamentoId, "Descrição do Apontamento de teste", DateTime.Now, 8, tarefaId, recursoId);
    }

    public static RemoveApontamentoCommand GetNewRemoveApontamentoCommand(Guid apontamentoId = default)
    {
        return new RemoveApontamentoCommand(apontamentoId == Guid.Empty ? Guid.NewGuid() : apontamentoId);
    }

    public static UpdateApontamentoCommand GetNewUpdateApontamentoCommand(Guid tarefaId, Guid recursoId, Guid apontamentoId = default)
    {
        return new UpdateApontamentoCommand(apontamentoId == Guid.Empty ? Guid.NewGuid() : apontamentoId, "Descrição do Apontamento de teste - alterado", DateTime.Now, 10, tarefaId, recursoId);
    }

    public static CreateTarefaCommand GetNewCreateTarefaCommand(Guid projetoId, Guid workflowId, Guid recursoId, Guid tipoTarefaId, Guid tarefaId = default)
    {
        return new CreateTarefaCommand(tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId, "Tarefa de teste", DateTime.Now, DateTime.Now.AddDays(5), 40, string.Empty, projetoId, workflowId, recursoId, tipoTarefaId);
    }

    public static RemoveTarefaCommand GetNewRemoveTarefaCommand(Guid tarefaId = default)
    {
        return new RemoveTarefaCommand(tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId);
    }

    public static UpdateTarefaCommand GetNewUpdateTarefaCommand(Guid projetoId, Guid workflowId, Guid recursoId, Guid tipoTarefaId, Guid tarefaId = default)
    {
        return new UpdateTarefaCommand(tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId, "Tarefa de teste - alterado", DateTime.Now, DateTime.Now.AddDays(8), 55, string.Empty, projetoId, workflowId, recursoId, tipoTarefaId);
    }

    public static UpdateTarefaByWorkflowCommand GetNewUpdateTarefaByWorkflowCommand(Guid workflowId, Guid tarefaId = default)
    {
        return new UpdateTarefaByWorkflowCommand(tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId, workflowId);
    }

    public static CreateTipoTarefaCommand GetNewCreateTipoTarefaCommand(Guid tipoTarefaId = default)
    {
        return new CreateTipoTarefaCommand(tipoTarefaId == Guid.Empty ? Guid.NewGuid() : tipoTarefaId, "Tipo Tarefa de teste", "success.png");
    }

    public static RemoveTipoTarefaCommand GetNewRemoveTipoTarefaCommand(Guid tipoTarefaId = default)
    {
        return new RemoveTipoTarefaCommand(tipoTarefaId == Guid.Empty ? Guid.NewGuid() : tipoTarefaId);
    }

    public static UpdateTipoTarefaCommand GetNewUpdateTipoTarefaCommand(Guid tipoTarefaId = default)
    {
        return new UpdateTipoTarefaCommand(tipoTarefaId == Guid.Empty ? Guid.NewGuid() : tipoTarefaId, "Tipo Tarefa de teste - alterado", "warning.png");
    }

    public static CreateImpedimentoTarefaCommand GetNewCreateImpedimentoTarefaCommand(Guid tarefaId, Guid impedimentoId, Guid impedimentoTarefaId = default)
    {
        return new CreateImpedimentoTarefaCommand(impedimentoTarefaId == Guid.Empty ? Guid.NewGuid() : impedimentoTarefaId, "Descrição do Impedimento Tarefa de teste", tarefaId, impedimentoId);
    }

    public static RemoveImpedimentoTarefaCommand GetNewRemoveImpedimentoTarefaCommand(Guid impedimentoTarefaId = default)
    {
        return new RemoveImpedimentoTarefaCommand(impedimentoTarefaId == Guid.Empty ? Guid.NewGuid() : impedimentoTarefaId);
    }

    public static UpdateImpedimentoTarefaCommand GetNewUpdateImpedimentoTarefaCommand(Guid tarefaId, Guid impedimentoId, Guid impedimentoTarefaId = default)
    {
        return new UpdateImpedimentoTarefaCommand(impedimentoTarefaId == Guid.Empty ? Guid.NewGuid() : impedimentoTarefaId, "Descrição do Impedimento Tarefa de teste - alterado", tarefaId, impedimentoId);
    }

    public static CreateRecursoProjetoCommand GetNewCreateRecursoProjetoCommand(Guid projetoId, Guid recursoId, Guid recursoProjetoId = default)
    {
        return new CreateRecursoProjetoCommand(recursoProjetoId == Guid.Empty ? Guid.NewGuid() : recursoProjetoId, recursoId, projetoId);
    }

    public static RemoveRecursoProjetoCommand GetNewRemoveRecursoProjetoCommand(Guid recursoProjetoId = default)
    {
        return new RemoveRecursoProjetoCommand(recursoProjetoId == Guid.Empty ? Guid.NewGuid() : recursoProjetoId);
    }

    public static UpdateRecursoProjetoCommand GetNewUpdateRecursoProjetoCommand(Guid projetoId, Guid recursoId, Guid recursoProjetoId = default)
    {
        return new UpdateRecursoProjetoCommand(recursoProjetoId == Guid.Empty ? Guid.NewGuid() : recursoProjetoId, recursoId, projetoId);
    }

    public static CreateRecursoTarefaCommand GetNewCreateRecursoTarefaCommand(Guid tarefaId, Guid recursoId, Guid recursoTarefaId = default)
    {
        return new CreateRecursoTarefaCommand(recursoTarefaId == Guid.Empty ? Guid.NewGuid() : recursoTarefaId, recursoId, tarefaId);
    }

    public static RemoveRecursoTarefaCommand GetNewRemoveRecursoTarefaCommand(Guid recursoTarefaId = default)
    {
        return new RemoveRecursoTarefaCommand(recursoTarefaId == Guid.Empty ? Guid.NewGuid() : recursoTarefaId);
    }

    public static UpdateRecursoTarefaCommand GetNewUpdateRecursoTarefaCommand(Guid tarefaId, Guid recursoId, Guid recursoTarefaId = default)
    {
        return new UpdateRecursoTarefaCommand(recursoTarefaId == Guid.Empty ? Guid.NewGuid() : recursoTarefaId, recursoId, tarefaId);
    }
}