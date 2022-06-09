namespace Cpnucleo.Application.Test.Helpers;

public class MockCommandHelper
{
    public static CreateSistemaCommand GetNewCreateSistemaCommand(Guid sistemaId = default)
    {
        return new CreateSistemaCommand
        {
            Id = sistemaId == Guid.Empty ? Guid.NewGuid() : sistemaId,
            Nome = "Sistema de teste",
            Descricao = "Descrição do sistema de teste"
        };
    }

    public static RemoveSistemaCommand GetNewRemoveSistemaCommand(Guid sistemaId = default)
    {
        return new RemoveSistemaCommand
        {
            Id = sistemaId == Guid.Empty ? Guid.NewGuid() : sistemaId
        };
    }

    public static UpdateSistemaCommand GetNewUpdateSistemaCommand(Guid sistemaId = default)
    {
        return new UpdateSistemaCommand
        {
            Id = sistemaId == Guid.Empty ? Guid.NewGuid() : sistemaId,
            Nome = "Sistema de teste - alterado",
            Descricao = "Descrição do sistema de teste - alterado"
        };
    }

    public static CreateProjetoCommand GetNewCreateProjetoCommand(Guid sistemaId, Guid projetoId = default)
    {
        return new CreateProjetoCommand
        {
            Id = projetoId == Guid.Empty ? Guid.NewGuid() : projetoId,
            Nome = "Projeto de teste",
            IdSistema = sistemaId
        };
    }

    public static RemoveProjetoCommand GetNewRemoveProjetoCommand(Guid projetoId = default)
    {
        return new RemoveProjetoCommand
        {
            Id = projetoId == Guid.Empty ? Guid.NewGuid() : projetoId
        };
    }

    public static UpdateProjetoCommand GetNewUpdateProjetoCommand(Guid sistemaId, Guid projetoId = default)
    {
        return new UpdateProjetoCommand
        {
            Id = projetoId == Guid.Empty ? Guid.NewGuid() : projetoId,
            Nome = "Projeto de teste - alterado",
            IdSistema = sistemaId
        };
    }

    public static CreateWorkflowCommand GetNewCreateWorkflowCommand(Guid workflowId = default)
    {
        return new CreateWorkflowCommand
        {
            Id = workflowId == Guid.Empty ? Guid.NewGuid() : workflowId,
            Nome = "Workflow de teste",
            Ordem = 1
        };
    }

    public static RemoveWorkflowCommand GetNewRemoveWorkflowCommand(Guid workflowId = default)
    {
        return new RemoveWorkflowCommand
        {
            Id = workflowId == Guid.Empty ? Guid.NewGuid() : workflowId
        };
    }

    public static UpdateWorkflowCommand GetNewUpdateWorkflowCommand(Guid workflowId = default)
    {
        return new UpdateWorkflowCommand
        {
            Id = workflowId == Guid.Empty ? Guid.NewGuid() : workflowId,
            Nome = "Workflow de teste - alterado",
            Ordem = 2
        };
    }

    public static CreateImpedimentoCommand GetNewCreateImpedimentoCommand(Guid impedimentoId = default)
    {
        return new CreateImpedimentoCommand
        {
            Id = impedimentoId == Guid.Empty ? Guid.NewGuid() : impedimentoId,
            Nome = "Impedimento de teste"
        };
    }

    public static RemoveImpedimentoCommand GetNewRemoveImpedimentoCommand(Guid impedimentoId = default)
    {
        return new RemoveImpedimentoCommand
        {
            Id = impedimentoId == Guid.Empty ? Guid.NewGuid() : impedimentoId
        };
    }

    public static UpdateImpedimentoCommand GetNewUpdateImpedimentoCommand(Guid impedimentoId = default)
    {
        return new UpdateImpedimentoCommand
        {
            Id = impedimentoId == Guid.Empty ? Guid.NewGuid() : impedimentoId,
            Nome = "Impedimento de teste - alterado"
        };
    }

    public static CreateRecursoCommand GetNewCreateRecursoCommand(Guid recursoId = default)
    {
        return new CreateRecursoCommand
        {
            Id = recursoId == Guid.Empty ? Guid.NewGuid() : recursoId,
            Nome = "Recurso de teste",
            Login = "usuario.teste",
            Senha = "12345678"
        };
    }

    public static RemoveRecursoCommand GetNewRemoveRecursoCommand(Guid recursoId = default)
    {
        return new RemoveRecursoCommand
        {
            Id = recursoId == Guid.Empty ? Guid.NewGuid() : recursoId
        };
    }

    public static UpdateRecursoCommand GetNewUpdateRecursoCommand(Guid recursoId = default)
    {
        return new UpdateRecursoCommand
        {
            Id = recursoId == Guid.Empty ? Guid.NewGuid() : recursoId,
            Nome = "Recurso de teste - alterado",
            Senha = "1234567890"
        };
    }

    public static CreateApontamentoCommand GetNewCreateApontamentoCommand(Guid tarefaId, Guid recursoId, Guid apontamentoId = default)
    {
        return new CreateApontamentoCommand
        {
            Id = apontamentoId == Guid.Empty ? Guid.NewGuid() : apontamentoId,
            Descricao = "Descrição do Apontamento de teste",
            DataApontamento = DateTime.Now,
            QtdHoras = 8,
            IdTarefa = tarefaId,
            IdRecurso = recursoId
        };
    }

    public static RemoveApontamentoCommand GetNewRemoveApontamentoCommand(Guid apontamentoId = default)
    {
        return new RemoveApontamentoCommand
        {
            Id = apontamentoId == Guid.Empty ? Guid.NewGuid() : apontamentoId
        };
    }

    public static UpdateApontamentoCommand GetNewUpdateApontamentoCommand(Guid tarefaId, Guid recursoId, Guid apontamentoId = default)
    {
        return new UpdateApontamentoCommand
        {
            Id = apontamentoId == Guid.Empty ? Guid.NewGuid() : apontamentoId,
            Descricao = "Descrição do Apontamento de teste - alterado",
            DataApontamento = DateTime.Now,
            QtdHoras = 10,
            IdTarefa = tarefaId,
            IdRecurso = recursoId
        };
    }

    public static CreateTarefaCommand GetNewCreateTarefaCommand(Guid projetoId, Guid workflowId, Guid recursoId, Guid tipoTarefaId, Guid tarefaId = default)
    {
        return new CreateTarefaCommand
        {
            Id = tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId,
            Nome = "Tarefa de teste",
            DataInicio = DateTime.Now,
            DataTermino = DateTime.Now.AddDays(5),
            QtdHoras = 40,
            IdProjeto = projetoId,
            IdWorkflow = workflowId,
            IdRecurso = recursoId,
            IdTipoTarefa = tipoTarefaId
        };
    }

    public static RemoveTarefaCommand GetNewRemoveTarefaCommand(Guid tarefaId = default)
    {
        return new RemoveTarefaCommand
        {
            Id = tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId
        };
    }

    public static UpdateTarefaCommand GetNewUpdateTarefaCommand(Guid projetoId, Guid workflowId, Guid recursoId, Guid tipoTarefaId, Guid tarefaId = default)
    {
        return new UpdateTarefaCommand
        {
            Id = tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId,
            Nome = "Tarefa de teste - alterado",
            DataInicio = DateTime.Now,
            DataTermino = DateTime.Now.AddDays(8),
            QtdHoras = 55,
            IdProjeto = projetoId,
            IdWorkflow = workflowId,
            IdRecurso = recursoId,
            IdTipoTarefa = tipoTarefaId
        };
    }

    public static UpdateTarefaByWorkflowCommand GetNewUpdateTarefaByWorkflowCommand(Guid workflowId, Guid tarefaId = default)
    {
        return new UpdateTarefaByWorkflowCommand
        {
            Id = tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId,
            IdWorkflow = workflowId
        };
    }

    public static CreateTipoTarefaCommand GetNewCreateTipoTarefaCommand(Guid tipoTarefaId = default)
    {
        return new CreateTipoTarefaCommand
        {
            Id = tipoTarefaId == Guid.Empty ? Guid.NewGuid() : tipoTarefaId,
            Nome = "Tipo Tarefa de teste",
            Image = "success.png"
        };
    }

    public static RemoveTipoTarefaCommand GetNewRemoveTipoTarefaCommand(Guid tipoTarefaId = default)
    {
        return new RemoveTipoTarefaCommand
        {
            Id = tipoTarefaId == Guid.Empty ? Guid.NewGuid() : tipoTarefaId
        };
    }

    public static UpdateTipoTarefaCommand GetNewUpdateTipoTarefaCommand(Guid tipoTarefaId = default)
    {
        return new UpdateTipoTarefaCommand
        {
            Id = tipoTarefaId == Guid.Empty ? Guid.NewGuid() : tipoTarefaId,
            Nome = "Tipo Tarefa de teste - alterado",
            Image = "warning.png"
        };
    }

    public static CreateImpedimentoTarefaCommand GetNewCreateImpedimentoTarefaCommand(Guid tarefaId, Guid impedimentoId, Guid impedimentoTarefaId = default)
    {
        return new CreateImpedimentoTarefaCommand
        {
            Id = impedimentoTarefaId == Guid.Empty ? Guid.NewGuid() : impedimentoTarefaId,
            Descricao = "Descrição do Impedimento Tarefa de teste",
            IdTarefa = tarefaId,
            IdImpedimento = impedimentoId
        };
    }

    public static RemoveImpedimentoTarefaCommand GetNewRemoveImpedimentoTarefaCommand(Guid impedimentoTarefaId = default)
    {
        return new RemoveImpedimentoTarefaCommand
        {
            Id = impedimentoTarefaId == Guid.Empty ? Guid.NewGuid() : impedimentoTarefaId
        };
    }

    public static UpdateImpedimentoTarefaCommand GetNewUpdateImpedimentoTarefaCommand(Guid tarefaId, Guid impedimentoId, Guid impedimentoTarefaId = default)
    {
        return new UpdateImpedimentoTarefaCommand
        {
            Id = impedimentoTarefaId == Guid.Empty ? Guid.NewGuid() : impedimentoTarefaId,
            Descricao = "Descrição do Impedimento Tarefa de teste - alterado",
            IdTarefa = tarefaId,
            IdImpedimento = impedimentoId
        };
    }

    public static CreateRecursoProjetoCommand GetNewCreateRecursoProjetoCommand(Guid projetoId, Guid recursoId, Guid recursoProjetoId = default)
    {
        return new CreateRecursoProjetoCommand
        {
            Id = recursoProjetoId == Guid.Empty ? Guid.NewGuid() : recursoProjetoId,
            IdProjeto = projetoId,
            IdRecurso = recursoId
        };
    }

    public static RemoveRecursoProjetoCommand GetNewRemoveRecursoProjetoCommand(Guid recursoProjetoId = default)
    {
        return new RemoveRecursoProjetoCommand
        {
            Id = recursoProjetoId == Guid.Empty ? Guid.NewGuid() : recursoProjetoId
        };
    }

    public static UpdateRecursoProjetoCommand GetNewUpdateRecursoProjetoCommand(Guid projetoId, Guid recursoId, Guid recursoProjetoId = default)
    {
        return new UpdateRecursoProjetoCommand
        {
            Id = recursoProjetoId == Guid.Empty ? Guid.NewGuid() : recursoProjetoId,
            IdProjeto = projetoId,
            IdRecurso = recursoId
        };
    }

    public static CreateRecursoTarefaCommand GetNewCreateRecursoTarefaCommand(Guid tarefaId, Guid recursoId, Guid recursoTarefaId = default)
    {
        return new CreateRecursoTarefaCommand
        {
            Id = recursoTarefaId == Guid.Empty ? Guid.NewGuid() : recursoTarefaId,
            IdTarefa = tarefaId,
            IdRecurso = recursoId
        };
    }

    public static RemoveRecursoTarefaCommand GetNewRemoveRecursoTarefaCommand(Guid recursoTarefaId = default)
    {
        return new RemoveRecursoTarefaCommand
        {
            Id = recursoTarefaId == Guid.Empty ? Guid.NewGuid() : recursoTarefaId
        };
    }

    public static UpdateRecursoTarefaCommand GetNewUpdateRecursoTarefaCommand(Guid tarefaId, Guid recursoId, Guid recursoTarefaId = default)
    {
        return new UpdateRecursoTarefaCommand
        {
            Id = recursoTarefaId == Guid.Empty ? Guid.NewGuid() : recursoTarefaId,
            IdTarefa = tarefaId,
            IdRecurso = recursoId
        };
    }
}