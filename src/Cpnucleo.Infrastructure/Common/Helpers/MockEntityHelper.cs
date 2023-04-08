namespace Cpnucleo.Infrastructure.Common.Helpers;

internal sealed class MockEntityHelper
{
    internal static DateTime defaultDate = DateTime.SpecifyKind(new DateTime(2000, 1, 1), DateTimeKind.Utc);

    internal static Apontamento GetNewApontamento(Guid tarefaId, Guid recursoId, Guid apontamentoId = default)
    {
        return new Apontamento
        {
            Id = apontamentoId == Guid.Empty ? Guid.NewGuid() : apontamentoId,
            Descricao = "Descrição do Apontamento de teste",
            DataApontamento = defaultDate,
            QtdHoras = 8,
            IdTarefa = tarefaId,
            IdRecurso = recursoId,
            DataInclusao = defaultDate,
            Ativo = true
        };
    }

    internal static Impedimento GetNewImpedimento(Guid impedimentoId = default)
    {
        return new Impedimento
        {
            Id = impedimentoId == Guid.Empty ? Guid.NewGuid() : impedimentoId,
            Nome = "Impedimento de teste",
            DataInclusao = defaultDate,
            Ativo = true
        };
    }

    internal static ImpedimentoTarefa GetNewImpedimentoTarefa(Guid tarefaId, Guid impedimentoId, Guid impedimentoTarefaId = default)
    {
        return new ImpedimentoTarefa
        {
            Id = impedimentoTarefaId == Guid.Empty ? Guid.NewGuid() : impedimentoTarefaId,
            Descricao = "Descrição do Impedimento Tarefa de teste",
            IdTarefa = tarefaId,
            IdImpedimento = impedimentoId,
            DataInclusao = defaultDate,
            Ativo = true
        };
    }

    internal static Projeto GetNewProjeto(Guid sistemaId, Guid projetoId = default)
    {
        return new Projeto
        {
            Id = projetoId == Guid.Empty ? Guid.NewGuid() : projetoId,
            Nome = "Projeto de teste",
            IdSistema = sistemaId,
            DataInclusao = defaultDate,
            Ativo = true
        };
    }

    internal static Recurso GetNewRecurso(Guid recursoId = default)
    {
        return new Recurso
        {
            Id = recursoId == Guid.Empty ? Guid.NewGuid() : recursoId,
            Nome = "Recurso de teste",
            Login = "usuario.teste",
            Senha = "k8n3YJ7em+uo32BbpRNgjB+kX6uRCJLN7V1L4Q7WwUqDrpz00uCHi+wOLJBZJkOQ",
            Salt = "okVTEMBEAbjnjKmD3On1qKwDT0+vfBRAzDM/T7vHqH+gZJxV8/9rRhqiALLlLC7r",
            DataInclusao = defaultDate,
            Ativo = true
        };
    }

    internal static RecursoProjeto GetNewRecursoProjeto(Guid projetoId, Guid recursoId, Guid recursoProjetoId = default)
    {
        return new RecursoProjeto
        {
            Id = recursoProjetoId == Guid.Empty ? Guid.NewGuid() : recursoProjetoId,
            IdProjeto = projetoId,
            IdRecurso = recursoId,
            DataInclusao = defaultDate,
            Ativo = true
        };
    }

    internal static RecursoTarefa GetNewRecursoTarefa(Guid tarefaId, Guid recursoId, Guid recursoTarefaId = default)
    {
        return new RecursoTarefa
        {
            Id = recursoTarefaId == Guid.Empty ? Guid.NewGuid() : recursoTarefaId,
            PercentualTarefa = 25,
            IdTarefa = tarefaId,
            IdRecurso = recursoId,
            DataInclusao = defaultDate,
            Ativo = true
        };
    }

    internal static Sistema GetNewSistema(Guid sistemaId = default)
    {
        return new Sistema
        {
            Id = sistemaId == Guid.Empty ? Guid.NewGuid() : sistemaId,
            Nome = "Sistema de teste",
            Descricao = "Descrição do sistema de teste",
            DataInclusao = defaultDate,
            Ativo = true
        };
    }

    internal static Tarefa GetNewTarefa(Guid projetoId, Guid workflowId, Guid recursoId, Guid tipoTarefaId, Guid tarefaId = default)
    {
        return new Tarefa
        {
            Id = tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId,
            Nome = "Sistema de teste",
            DataInicio = defaultDate,
            DataTermino = defaultDate.AddDays(5),
            QtdHoras = 40,
            IdProjeto = projetoId,
            IdWorkflow = workflowId,
            IdRecurso = recursoId,
            IdTipoTarefa = tipoTarefaId,
            DataInclusao = defaultDate,
            Ativo = true
        };
    }

    internal static TipoTarefa GetNewTipoTarefa(string nome, string image, Guid tipoTarefaId = default)
    {
        return new TipoTarefa
        {
            Id = tipoTarefaId == Guid.Empty ? Guid.NewGuid() : tipoTarefaId,
            Nome = nome,
            Image = image,
            DataInclusao = defaultDate,
            Ativo = true
        };
    }

    internal static Workflow GetNewWorkflow(string nome, int ordem, Guid workflowId = default)
    {
        return new Workflow
        {
            Id = workflowId == Guid.Empty ? Guid.NewGuid() : workflowId,
            Nome = nome,
            Ordem = ordem,
            DataInclusao = defaultDate,
            Ativo = true
        };
    }
}
