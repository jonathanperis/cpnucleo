using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;

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

















































































































    //public static ApontamentoViewModel GetNewApontamento(Guid tarefaId, Guid recursoId, Guid apontamentoId = default, DateTime dataInclusao = default)
    //{
    //    return new ApontamentoViewModel
    //    {
    //        Id = apontamentoId == Guid.Empty ? Guid.NewGuid() : apontamentoId,
    //        Descricao = "Descrição do Apontamento de teste",
    //        DataApontamento = DateTime.Now,
    //        QtdHoras = 8,
    //        IdTarefa = tarefaId,
    //        IdRecurso = recursoId,
    //        DataInclusao = dataInclusao
    //    };
    //}

    //public static ImpedimentoViewModel GetNewImpedimento(Guid impedimentoId = default, DateTime dataInclusao = default)
    //{
    //    return new ImpedimentoViewModel
    //    {
    //        Id = impedimentoId == Guid.Empty ? Guid.NewGuid() : impedimentoId,
    //        Nome = "Impedimento de teste",
    //        DataInclusao = dataInclusao
    //    };
    //}

    //public static ImpedimentoTarefaViewModel GetNewImpedimentoTarefa(Guid tarefaId, Guid impedimentoId, Guid impedimentoTarefaId = default, DateTime dataInclusao = default)
    //{
    //    return new ImpedimentoTarefaViewModel
    //    {
    //        Id = impedimentoTarefaId == Guid.Empty ? Guid.NewGuid() : impedimentoTarefaId,
    //        Descricao = "Descrição do Impedimento Tarefa de teste",
    //        DataInclusao = dataInclusao,
    //        IdTarefa = tarefaId,
    //        IdImpedimento = impedimentoId
    //    };
    //}

    //public static ProjetoViewModel GetNewProjeto(Guid sistemaId, Guid projetoId = default, DateTime dataInclusao = default)
    //{
    //    return new ProjetoViewModel
    //    {
    //        Id = projetoId == Guid.Empty ? Guid.NewGuid() : projetoId,
    //        Nome = "Projeto de teste",
    //        IdSistema = sistemaId,
    //        DataInclusao = dataInclusao
    //    };
    //}

    //public static RecursoViewModel GetNewRecurso(Guid recursoId = default, DateTime dataInclusao = default)
    //{
    //    return new RecursoViewModel
    //    {
    //        Id = recursoId == Guid.Empty ? Guid.NewGuid() : recursoId,
    //        Nome = "Recurso de teste",
    //        Login = "usuario.teste",
    //        Senha = "12345678",
    //        DataInclusao = dataInclusao
    //    };
    //}

    //public static RecursoProjetoViewModel GetNewRecursoProjeto(Guid projetoId, Guid recursoId, Guid recursoProjetoId = default, DateTime dataInclusao = default)
    //{
    //    return new RecursoProjetoViewModel
    //    {
    //        Id = recursoProjetoId == Guid.Empty ? Guid.NewGuid() : recursoProjetoId,
    //        IdProjeto = projetoId,
    //        IdRecurso = recursoId,
    //        DataInclusao = dataInclusao
    //    };
    //}

    //public static RecursoTarefaViewModel GetNewRecursoTarefa(Guid tarefaId, Guid recursoId, Guid recursoTarefaId = default, DateTime dataInclusao = default)
    //{
    //    return new RecursoTarefaViewModel
    //    {
    //        Id = recursoTarefaId == Guid.Empty ? Guid.NewGuid() : recursoTarefaId,
    //        IdTarefa = tarefaId,
    //        IdRecurso = recursoId,
    //        DataInclusao = dataInclusao
    //    };
    //}

    //public static SistemaViewModel GetNewSistema(Guid sistemaId = default, DateTime dataInclusao = default)
    //{
    //    return new SistemaViewModel
    //    {
    //        Id = sistemaId == Guid.Empty ? Guid.NewGuid() : sistemaId,
    //        Nome = "Sistema de teste",
    //        Descricao = "Descrição do sistema de teste",
    //        DataInclusao = dataInclusao
    //    };
    //}

    //public static TarefaViewModel GetNewTarefa(Guid projetoId, Guid workflowId, Guid recursoId, Guid tipoTarefaId, Guid tarefaId = default, DateTime dataInclusao = default)
    //{
    //    return new TarefaViewModel
    //    {
    //        Id = tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId,
    //        Nome = "Tarefa de teste",
    //        DataInicio = DateTime.Now,
    //        DataTermino = DateTime.Now.AddDays(5),
    //        QtdHoras = 40,
    //        IdProjeto = projetoId,
    //        IdWorkflow = workflowId,
    //        IdRecurso = recursoId,
    //        IdTipoTarefa = tipoTarefaId,
    //        DataInclusao = dataInclusao
    //    };
    //}

    //public static TipoTarefaViewModel GetNewTipoTarefa(Guid tipoTarefaId = default, DateTime dataInclusao = default)
    //{
    //    return new TipoTarefaViewModel
    //    {
    //        Id = tipoTarefaId == Guid.Empty ? Guid.NewGuid() : tipoTarefaId,
    //        Nome = "TipoTarefa de teste",
    //        Element = "success-element",
    //        Image = "success.png",
    //        DataInclusao = dataInclusao
    //    };
    //}

    //public static WorkflowViewModel GetNewWorkflow(Guid workflowId = default, DateTime dataInclusao = default)
    //{
    //    return new WorkflowViewModel
    //    {
    //        Id = workflowId == Guid.Empty ? Guid.NewGuid() : workflowId,
    //        Nome = "Workflow de teste",
    //        Ordem = 1,
    //        DataInclusao = dataInclusao
    //    };
    //}
}
