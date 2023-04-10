namespace Cpnucleo.Application.Test.Helpers;

public class MockCommandHelper
{
    public static CreateSistemaCommand GetNewCreateSistemaCommand()
    {
        return new CreateSistemaCommand("Sistema de teste", "Descrição do sistema de teste");
    }

    public static RemoveSistemaCommand GetNewRemoveSistemaCommand(Guid sistemaId)
    {
        return new RemoveSistemaCommand(sistemaId);
    }

    public static UpdateSistemaCommand GetNewUpdateSistemaCommand(Guid sistemaId)
    {
        return new UpdateSistemaCommand(sistemaId, "Sistema de teste - alterado", "Descrição do sistema de teste - alterado");
    }

    public static CreateProjetoCommand GetNewCreateProjetoCommand(Guid sistemaId)
    {
        return new CreateProjetoCommand("Projeto de teste", sistemaId);
    }

    public static RemoveProjetoCommand GetNewRemoveProjetoCommand(Guid projetoId)
    {
        return new RemoveProjetoCommand(projetoId);
    }

    public static UpdateProjetoCommand GetNewUpdateProjetoCommand(Guid sistemaId, Guid projetoId)
    {
        return new UpdateProjetoCommand(projetoId, "Projeto de teste - alterado", sistemaId);
    }

    public static CreateWorkflowCommand GetNewCreateWorkflowCommand()
    {
        return new CreateWorkflowCommand("Workflow de teste", 1);
    }

    public static RemoveWorkflowCommand GetNewRemoveWorkflowCommand(Guid workflowId)
    {
        return new RemoveWorkflowCommand(workflowId);
    }

    public static UpdateWorkflowCommand GetNewUpdateWorkflowCommand(Guid workflowId)
    {
        return new UpdateWorkflowCommand(workflowId, "Workflow de teste - alterado", 2);
    }

    public static CreateImpedimentoCommand GetNewCreateImpedimentoCommand()
    {
        return new CreateImpedimentoCommand("Impedimento de teste");
    }

    public static RemoveImpedimentoCommand GetNewRemoveImpedimentoCommand(Guid impedimentoId)
    {
        return new RemoveImpedimentoCommand(impedimentoId);
    }

    public static UpdateImpedimentoCommand GetNewUpdateImpedimentoCommand(Guid impedimentoId)
    {
        return new UpdateImpedimentoCommand(impedimentoId, "Impedimento de teste - alterado");
    }

    public static CreateRecursoCommand GetNewCreateRecursoCommand()
    {
        return new CreateRecursoCommand("Recurso de teste", "usuario.teste", "12345678");
    }

    public static RemoveRecursoCommand GetNewRemoveRecursoCommand(Guid recursoId)
    {
        return new RemoveRecursoCommand(recursoId);
    }

    public static UpdateRecursoCommand GetNewUpdateRecursoCommand(Guid recursoId)
    {
        return new UpdateRecursoCommand(recursoId, "Recurso de teste - alterado", "1234567890");
    }

    public static CreateApontamentoCommand GetNewCreateApontamentoCommand(Guid tarefaId, Guid recursoId)
    {
        return new CreateApontamentoCommand("Descrição do Apontamento de teste", DateTime.UtcNow, 8, tarefaId, recursoId);
    }

    public static RemoveApontamentoCommand GetNewRemoveApontamentoCommand(Guid apontamentoId)
    {
        return new RemoveApontamentoCommand(apontamentoId);
    }

    public static UpdateApontamentoCommand GetNewUpdateApontamentoCommand(Guid tarefaId, Guid recursoId, Guid apontamentoId)
    {
        return new UpdateApontamentoCommand(apontamentoId, "Descrição do Apontamento de teste - alterado", DateTime.UtcNow, 10, tarefaId, recursoId);
    }

    public static CreateTarefaCommand GetNewCreateTarefaCommand(Guid projetoId, Guid workflowId, Guid recursoId, Guid tipoTarefaId)
    {
        return new CreateTarefaCommand("Tarefa de teste", DateTime.UtcNow, DateTime.UtcNow.AddDays(5), 40, string.Empty, projetoId, workflowId, recursoId, tipoTarefaId);
    }

    public static RemoveTarefaCommand GetNewRemoveTarefaCommand(Guid tarefaId)
    {
        return new RemoveTarefaCommand(tarefaId);
    }

    public static UpdateTarefaCommand GetNewUpdateTarefaCommand(Guid projetoId, Guid workflowId, Guid recursoId, Guid tipoTarefaId, Guid tarefaId)
    {
        return new UpdateTarefaCommand(tarefaId, "Tarefa de teste - alterado", DateTime.UtcNow, DateTime.UtcNow.AddDays(8), 55, string.Empty, projetoId, workflowId, recursoId, tipoTarefaId);
    }

    public static UpdateTarefaByWorkflowCommand GetNewUpdateTarefaByWorkflowCommand(Guid workflowId, Guid tarefaId)
    {
        return new UpdateTarefaByWorkflowCommand(tarefaId, workflowId);
    }

    public static CreateTipoTarefaCommand GetNewCreateTipoTarefaCommand()
    {
        return new CreateTipoTarefaCommand("Tipo Tarefa de teste", "success.png");
    }

    public static RemoveTipoTarefaCommand GetNewRemoveTipoTarefaCommand(Guid tipoTarefaId)
    {
        return new RemoveTipoTarefaCommand(tipoTarefaId);
    }

    public static UpdateTipoTarefaCommand GetNewUpdateTipoTarefaCommand(Guid tipoTarefaId)
    {
        return new UpdateTipoTarefaCommand(tipoTarefaId, "Tipo Tarefa de teste - alterado", "warning.png");
    }

    public static CreateImpedimentoTarefaCommand GetNewCreateImpedimentoTarefaCommand(Guid tarefaId, Guid impedimentoId)
    {
        return new CreateImpedimentoTarefaCommand("Descrição do Impedimento Tarefa de teste", tarefaId, impedimentoId);
    }

    public static RemoveImpedimentoTarefaCommand GetNewRemoveImpedimentoTarefaCommand(Guid impedimentoTarefaId)
    {
        return new RemoveImpedimentoTarefaCommand(impedimentoTarefaId);
    }

    public static UpdateImpedimentoTarefaCommand GetNewUpdateImpedimentoTarefaCommand(Guid tarefaId, Guid impedimentoId, Guid impedimentoTarefaId)
    {
        return new UpdateImpedimentoTarefaCommand(impedimentoTarefaId, "Descrição do Impedimento Tarefa de teste - alterado", tarefaId, impedimentoId);
    }

    public static CreateRecursoProjetoCommand GetNewCreateRecursoProjetoCommand(Guid projetoId, Guid recursoId)
    {
        return new CreateRecursoProjetoCommand(recursoId, projetoId);
    }

    public static RemoveRecursoProjetoCommand GetNewRemoveRecursoProjetoCommand(Guid recursoProjetoId)
    {
        return new RemoveRecursoProjetoCommand(recursoProjetoId);
    }

    public static UpdateRecursoProjetoCommand GetNewUpdateRecursoProjetoCommand(Guid projetoId, Guid recursoId, Guid recursoProjetoId)
    {
        return new UpdateRecursoProjetoCommand(recursoProjetoId, recursoId, projetoId);
    }

    public static CreateRecursoTarefaCommand GetNewCreateRecursoTarefaCommand(Guid tarefaId, Guid recursoId)
    {
        return new CreateRecursoTarefaCommand(recursoId, tarefaId);
    }

    public static RemoveRecursoTarefaCommand GetNewRemoveRecursoTarefaCommand(Guid recursoTarefaId)
    {
        return new RemoveRecursoTarefaCommand(recursoTarefaId);
    }

    public static UpdateRecursoTarefaCommand GetNewUpdateRecursoTarefaCommand(Guid tarefaId, Guid recursoId, Guid recursoTarefaId)
    {
        return new UpdateRecursoTarefaCommand(recursoTarefaId, recursoId, tarefaId);
    }
}