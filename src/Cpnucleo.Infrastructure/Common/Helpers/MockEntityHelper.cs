namespace Cpnucleo.Infrastructure.Common.Helpers;

internal sealed class MockEntityHelper
{
    internal static DateTime defaultDate = DateTime.SpecifyKind(new DateTime(2000, 1, 1), DateTimeKind.Utc);

    internal static Apontamento GetNewApontamento(Guid tarefaId, Guid recursoId, Guid impedimentoId = default)
    {
        return Apontamento.Create("Descrição do Apontamento de teste",
                                  defaultDate,
                                  8,
                                  tarefaId,
                                  recursoId,
                                  impedimentoId);
    }

    internal static Impedimento GetNewImpedimento(Guid impedimentoId = default)
    {
        return Impedimento.Create("Impedimento de teste", impedimentoId);
    }

    internal static ImpedimentoTarefa GetNewImpedimentoTarefa(Guid tarefaId, Guid impedimentoId, Guid impedimentoTarefaId = default)
    {
        return ImpedimentoTarefa.Create("Descrição do Impedimento Tarefa de teste", tarefaId, impedimentoId, impedimentoTarefaId);
    }

    internal static Projeto GetNewProjeto(Guid sistemaId, Guid projetoId = default)
    {
        return Projeto.Create("Projeto de teste", sistemaId, projetoId);
    }

    internal static Recurso GetNewRecurso(Guid recursoId = default)
    {
        return Recurso.Create("Recurso de teste", "usuario.teste", "12345678", recursoId);
    }

    internal static RecursoProjeto GetNewRecursoProjeto(Guid projetoId, Guid recursoId, Guid recursoProjetoId = default)
    {
        return RecursoProjeto.Create(projetoId, recursoId, recursoProjetoId);
    }

    internal static RecursoTarefa GetNewRecursoTarefa(Guid tarefaId, Guid recursoId, Guid recursoTarefaId = default)
    {
        return RecursoTarefa.Create(tarefaId, recursoId, recursoTarefaId);
    }

    internal static Sistema GetNewSistema(Guid sistemaId = default)
    {
        return Sistema.Create("Sistema de teste", "Descrição do sistema de teste", sistemaId);
    }

    internal static Tarefa GetNewTarefa(Guid projetoId, Guid workflowId, Guid recursoId, Guid tipoTarefaId, Guid tarefaId = default)
    {
        return Tarefa.Create("Sistema de teste",
                             defaultDate,
                             defaultDate.AddDays(5),
                             40,
                             "Detalhe da tarefa de teste",
                             projetoId,
                             workflowId,
                             recursoId,
                             tipoTarefaId,
                             tarefaId);
    }

    internal static TipoTarefa GetNewTipoTarefa(string nome, string image, Guid tipoTarefaId = default)
    {
        return TipoTarefa.Create(nome, image, tipoTarefaId);
    }

    internal static Workflow GetNewWorkflow(string nome, int ordem, Guid workflowId = default)
    {
        return Workflow.Create(nome, ordem, workflowId);
    }
}
