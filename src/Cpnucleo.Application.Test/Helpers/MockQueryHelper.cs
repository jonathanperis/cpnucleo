namespace Cpnucleo.Application.Test.Helpers;

public class MockQueryHelper
{
    public static GetSistemaQuery GetNewGetSistemaQuery(Guid sistemaId = default)
    {
        return new GetSistemaQuery
        {
            Id = sistemaId == Guid.Empty ? Guid.NewGuid() : sistemaId
        };
    }

    public static ListSistemaQuery GetNewListSistemaQuery(bool getDependencies = default)
    {
        return new ListSistemaQuery
        {
            GetDependencies = getDependencies
        };
    }

    public static GetProjetoQuery GetNewGetProjetoQuery(Guid projetoId = default)
    {
        return new GetProjetoQuery
        {
            Id = projetoId == Guid.Empty ? Guid.NewGuid() : projetoId
        };
    }

    public static ListProjetoQuery GetNewListProjetoQuery(bool getDependencies = default)
    {
        return new ListProjetoQuery
        {
            GetDependencies = getDependencies
        };
    }

    public static GetWorkflowQuery GetNewGetWorkflowQuery(Guid workflowId = default)
    {
        return new GetWorkflowQuery
        {
            Id = workflowId == Guid.Empty ? Guid.NewGuid() : workflowId
        };
    }

    public static ListWorkflowQuery GetNewListWorkflowQuery(bool getDependencies = default)
    {
        return new ListWorkflowQuery
        {
            GetDependencies = getDependencies
        };
    }

    public static GetImpedimentoQuery GetNewGetImpedimentoQuery(Guid impedimentoId = default)
    {
        return new GetImpedimentoQuery
        {
            Id = impedimentoId == Guid.Empty ? Guid.NewGuid() : impedimentoId
        };
    }

    public static ListImpedimentoQuery GetNewListImpedimentoQuery(bool getDependencies = default)
    {
        return new ListImpedimentoQuery
        {
            GetDependencies = getDependencies
        };
    }

    public static GetRecursoQuery GetNewGetRecursoQuery(Guid recursoId = default)
    {
        return new GetRecursoQuery
        {
            Id = recursoId == Guid.Empty ? Guid.NewGuid() : recursoId
        };
    }

    public static ListRecursoQuery GetNewListRecursoQuery(bool getDependencies = default)
    {
        return new ListRecursoQuery
        {
            GetDependencies = getDependencies
        };
    }

    public static GetApontamentoQuery GetNewGetApontamentoQuery(Guid apontamentoId = default)
    {
        return new GetApontamentoQuery
        {
            Id = apontamentoId == Guid.Empty ? Guid.NewGuid() : apontamentoId
        };
    }

    public static GetApontamentoByRecursoQuery GetApontamentoByRecursoQuery(Guid recursoId = default)
    {
        return new GetApontamentoByRecursoQuery
        {
            IdRecurso = recursoId == Guid.Empty ? Guid.NewGuid() : recursoId
        };
    }

    public static ListApontamentoQuery GetNewListApontamentoQuery(bool getDependencies = default)
    {
        return new ListApontamentoQuery
        {
            GetDependencies = getDependencies
        };
    }

    public static GetTarefaQuery GetNewGetTarefaQuery(Guid tarefaId = default)
    {
        return new GetTarefaQuery
        {
            Id = tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId
        };
    }

    public static GetTarefaByRecursoQuery GetNewGetTarefaByRecursoQuery(Guid recursoId = default)
    {
        return new GetTarefaByRecursoQuery
        {
            IdRecurso = recursoId == Guid.Empty ? Guid.NewGuid() : recursoId
        };
    }

    public static ListTarefaQuery GetNewListTarefaQuery(bool getDependencies = default)
    {
        return new ListTarefaQuery
        {
            GetDependencies = getDependencies
        };
    }

    public static GetTipoTarefaQuery GetNewGetTipoTarefaQuery(Guid tipoTarefaId = default)
    {
        return new GetTipoTarefaQuery
        {
            Id = tipoTarefaId == Guid.Empty ? Guid.NewGuid() : tipoTarefaId
        };
    }

    public static ListTipoTarefaQuery GetNewListTipoTarefaQuery(bool getDependencies = default)
    {
        return new ListTipoTarefaQuery
        {
            GetDependencies = getDependencies
        };
    }

    public static GetImpedimentoTarefaQuery GetNewGetImpedimentoTarefaQuery(Guid impedimentoTarefaId = default)
    {
        return new GetImpedimentoTarefaQuery
        {
            Id = impedimentoTarefaId == Guid.Empty ? Guid.NewGuid() : impedimentoTarefaId
        };
    }

    public static GetImpedimentoTarefaByTarefaQuery GetNewGetImpedimentoTarefaByTarefaQuery(Guid tarefaId = default)
    {
        return new GetImpedimentoTarefaByTarefaQuery
        {
            IdTarefa = tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId
        };
    }

    public static ListImpedimentoTarefaQuery GetNewListImpedimentoTarefaQuery(bool getDependencies = default)
    {
        return new ListImpedimentoTarefaQuery
        {
            GetDependencies = getDependencies
        };
    }

    public static GetRecursoProjetoQuery GetNewGetRecursoProjetoQuery(Guid recursoProjetoId = default)
    {
        return new GetRecursoProjetoQuery
        {
            Id = recursoProjetoId == Guid.Empty ? Guid.NewGuid() : recursoProjetoId
        };
    }

    public static GetRecursoProjetoByProjetoQuery GetNewGetRecursoProjetoByProjetoQuery(Guid projetoId = default)
    {
        return new GetRecursoProjetoByProjetoQuery
        {
            IdProjeto = projetoId == Guid.Empty ? Guid.NewGuid() : projetoId
        };
    }

    public static ListRecursoProjetoQuery GetNewListRecursoProjetoQuery(bool getDependencies = default)
    {
        return new ListRecursoProjetoQuery
        {
            GetDependencies = getDependencies
        };
    }

    public static GetRecursoTarefaQuery GetNewGetRecursoTarefaQuery(Guid recursoTarefaId = default)
    {
        return new GetRecursoTarefaQuery
        {
            Id = recursoTarefaId == Guid.Empty ? Guid.NewGuid() : recursoTarefaId
        };
    }

    public static GetRecursoTarefaByTarefaQuery GetNewGetRecursoTarefaByTarefaQuery(Guid tarefaId = default)
    {
        return new GetRecursoTarefaByTarefaQuery
        {
            IdTarefa = tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId
        };
    }

    public static ListRecursoTarefaQuery GetNewListRecursoTarefaQuery(bool getDependencies = default)
    {
        return new ListRecursoTarefaQuery
        {
            GetDependencies = getDependencies
        };
    }
}