namespace Cpnucleo.Application.Test.Helpers;

public class MockQueryHelper
{
    public static GetSistemaQuery GetNewGetSistemaQuery(Guid sistemaId = default)
    {
        return new GetSistemaQuery(sistemaId == Guid.Empty ? Guid.NewGuid() : sistemaId);
    }

    public static ListSistemaQuery GetNewListSistemaQuery(bool getDependencies = false)
    {
        return new ListSistemaQuery(getDependencies);
    }

    public static GetProjetoQuery GetNewGetProjetoQuery(Guid projetoId = default)
    {
        return new GetProjetoQuery(projetoId == Guid.Empty ? Guid.NewGuid() : projetoId);
    }

    public static ListProjetoQuery GetNewListProjetoQuery(bool getDependencies = false)
    {
        return new ListProjetoQuery(getDependencies);
    }

    public static GetWorkflowQuery GetNewGetWorkflowQuery(Guid workflowId = default)
    {
        return new GetWorkflowQuery(workflowId == Guid.Empty ? Guid.NewGuid() : workflowId);
    }

    public static ListWorkflowQuery GetNewListWorkflowQuery(bool getDependencies = false)
    {
        return new ListWorkflowQuery(getDependencies);
    }

    public static GetImpedimentoQuery GetNewGetImpedimentoQuery(Guid impedimentoId = default)
    {
        return new GetImpedimentoQuery(impedimentoId == Guid.Empty ? Guid.NewGuid() : impedimentoId);
    }

    public static ListImpedimentoQuery GetNewListImpedimentoQuery(bool getDependencies = false)
    {
        return new ListImpedimentoQuery(getDependencies);
    }

    public static GetRecursoQuery GetNewGetRecursoQuery(Guid recursoId = default)
    {
        return new GetRecursoQuery(recursoId == Guid.Empty ? Guid.NewGuid() : recursoId);
    }

    public static ListRecursoQuery GetNewListRecursoQuery(bool getDependencies = false)
    {
        return new ListRecursoQuery(getDependencies);
    }

    public static GetApontamentoQuery GetNewGetApontamentoQuery(Guid apontamentoId = default)
    {
        return new GetApontamentoQuery(apontamentoId == Guid.Empty ? Guid.NewGuid() : apontamentoId);
    }

    public static ListApontamentoByRecursoQuery GetApontamentoByRecursoQuery(Guid recursoId = default)
    {
        return new ListApontamentoByRecursoQuery(recursoId == Guid.Empty ? Guid.NewGuid() : recursoId);
    }

    public static ListApontamentoQuery GetNewListApontamentoQuery(bool getDependencies = false)
    {
        return new ListApontamentoQuery(getDependencies);
    }

    public static GetTarefaQuery GetNewGetTarefaQuery(Guid tarefaId = default)
    {
        return new GetTarefaQuery(tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId);
    }

    public static ListTarefaByRecursoQuery GetNewGetTarefaByRecursoQuery(Guid recursoId = default)
    {
        return new ListTarefaByRecursoQuery(recursoId == Guid.Empty ? Guid.NewGuid() : recursoId);
    }

    public static ListTarefaQuery GetNewListTarefaQuery(bool getDependencies = false)
    {
        return new ListTarefaQuery(getDependencies);
    }

    public static GetTipoTarefaQuery GetNewGetTipoTarefaQuery(Guid tipoTarefaId = default)
    {
        return new GetTipoTarefaQuery(tipoTarefaId == Guid.Empty ? Guid.NewGuid() : tipoTarefaId);
    }

    public static ListTipoTarefaQuery GetNewListTipoTarefaQuery(bool getDependencies = false)
    {
        return new ListTipoTarefaQuery(getDependencies);
    }

    public static GetImpedimentoTarefaQuery GetNewGetImpedimentoTarefaQuery(Guid impedimentoTarefaId = default)
    {
        return new GetImpedimentoTarefaQuery(impedimentoTarefaId == Guid.Empty ? Guid.NewGuid() : impedimentoTarefaId);
    }

    public static ListImpedimentoTarefaByTarefaQuery GetNewGetImpedimentoTarefaByTarefaQuery(Guid tarefaId = default)
    {
        return new ListImpedimentoTarefaByTarefaQuery(tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId);
    }

    public static ListImpedimentoTarefaQuery GetNewListImpedimentoTarefaQuery(bool getDependencies = false)
    {
        return new ListImpedimentoTarefaQuery(getDependencies);
    }

    public static GetRecursoProjetoQuery GetNewGetRecursoProjetoQuery(Guid recursoProjetoId = default)
    {
        return new GetRecursoProjetoQuery(recursoProjetoId == Guid.Empty ? Guid.NewGuid() : recursoProjetoId);
    }

    public static ListRecursoProjetoByProjetoQuery GetNewGetRecursoProjetoByProjetoQuery(Guid projetoId = default)
    {
        return new ListRecursoProjetoByProjetoQuery(projetoId == Guid.Empty ? Guid.NewGuid() : projetoId);
    }

    public static ListRecursoProjetoQuery GetNewListRecursoProjetoQuery(bool getDependencies = false)
    {
        return new ListRecursoProjetoQuery(getDependencies);
    }

    public static GetRecursoTarefaQuery GetNewGetRecursoTarefaQuery(Guid recursoTarefaId = default)
    {
        return new GetRecursoTarefaQuery(recursoTarefaId == Guid.Empty ? Guid.NewGuid() : recursoTarefaId);
    }

    public static ListRecursoTarefaByTarefaQuery GetNewGetRecursoTarefaByTarefaQuery(Guid tarefaId = default)
    {
        return new ListRecursoTarefaByTarefaQuery(tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId);
    }

    public static ListRecursoTarefaQuery GetNewListRecursoTarefaQuery(bool getDependencies = false)
    {
        return new ListRecursoTarefaQuery(getDependencies);
    }
}