namespace Cpnucleo.Application.Test.Helpers;

public class MockQueryHelper
{
    public static GetSistemaQuery GetNewGetSistemaQuery(Guid sistemaId)
    {
        return new GetSistemaQuery(sistemaId);
    }

    public static ListSistemaQuery GetNewListSistemaQuery()
    {
        return new ListSistemaQuery();
    }

    public static GetProjetoQuery GetNewGetProjetoQuery(Guid projetoId)
    {
        return new GetProjetoQuery(projetoId);
    }

    public static ListProjetoQuery GetNewListProjetoQuery()
    {
        return new ListProjetoQuery();
    }

    public static GetWorkflowQuery GetNewGetWorkflowQuery(Guid workflowId)
    {
        return new GetWorkflowQuery(workflowId);
    }

    public static ListWorkflowQuery GetNewListWorkflowQuery()
    {
        return new ListWorkflowQuery();
    }

    public static GetImpedimentoQuery GetNewGetImpedimentoQuery(Guid impedimentoId)
    {
        return new GetImpedimentoQuery(impedimentoId);
    }

    public static ListImpedimentoQuery GetNewListImpedimentoQuery()
    {
        return new ListImpedimentoQuery();
    }

    public static GetRecursoQuery GetNewGetRecursoQuery(Guid recursoId)
    {
        return new GetRecursoQuery(recursoId);
    }

    public static ListRecursoQuery GetNewListRecursoQuery()
    {
        return new ListRecursoQuery();
    }

    public static GetApontamentoQuery GetNewGetApontamentoQuery(Guid apontamentoId)
    {
        return new GetApontamentoQuery(apontamentoId);
    }

    public static ListApontamentoByRecursoQuery GetApontamentoByRecursoQuery(Guid recursoId)
    {
        return new ListApontamentoByRecursoQuery(recursoId);
    }

    public static ListApontamentoQuery GetNewListApontamentoQuery()
    {
        return new ListApontamentoQuery();
    }

    public static GetTarefaQuery GetNewGetTarefaQuery(Guid tarefaId)
    {
        return new GetTarefaQuery(tarefaId);
    }

    public static ListTarefaByRecursoQuery GetNewGetTarefaByRecursoQuery(Guid recursoId)
    {
        return new ListTarefaByRecursoQuery(recursoId);
    }

    public static ListTarefaQuery GetNewListTarefaQuery()
    {
        return new ListTarefaQuery();
    }

    public static GetTipoTarefaQuery GetNewGetTipoTarefaQuery(Guid tipoTarefaId)
    {
        return new GetTipoTarefaQuery(tipoTarefaId);
    }

    public static ListTipoTarefaQuery GetNewListTipoTarefaQuery()
    {
        return new ListTipoTarefaQuery();
    }

    public static GetImpedimentoTarefaQuery GetNewGetImpedimentoTarefaQuery(Guid impedimentoTarefaId)
    {
        return new GetImpedimentoTarefaQuery(impedimentoTarefaId);
    }

    public static ListImpedimentoTarefaByTarefaQuery GetNewGetImpedimentoTarefaByTarefaQuery(Guid tarefaId)
    {
        return new ListImpedimentoTarefaByTarefaQuery(tarefaId);
    }

    public static ListImpedimentoTarefaQuery GetNewListImpedimentoTarefaQuery()
    {
        return new ListImpedimentoTarefaQuery();
    }

    public static GetRecursoProjetoQuery GetNewGetRecursoProjetoQuery(Guid recursoProjetoId)
    {
        return new GetRecursoProjetoQuery(recursoProjetoId);
    }

    public static ListRecursoProjetoByProjetoQuery GetNewGetRecursoProjetoByProjetoQuery(Guid projetoId)
    {
        return new ListRecursoProjetoByProjetoQuery(projetoId);
    }

    public static ListRecursoProjetoQuery GetNewListRecursoProjetoQuery()
    {
        return new ListRecursoProjetoQuery();
    }

    public static GetRecursoTarefaQuery GetNewGetRecursoTarefaQuery(Guid recursoTarefaId)
    {
        return new GetRecursoTarefaQuery(recursoTarefaId);
    }

    public static ListRecursoTarefaByTarefaQuery GetNewGetRecursoTarefaByTarefaQuery(Guid tarefaId)
    {
        return new ListRecursoTarefaByTarefaQuery(tarefaId);
    }

    public static ListRecursoTarefaQuery GetNewListRecursoTarefaQuery()
    {
        return new ListRecursoTarefaQuery();
    }
}