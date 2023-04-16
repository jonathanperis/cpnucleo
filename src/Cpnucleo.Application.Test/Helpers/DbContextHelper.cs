namespace Cpnucleo.Application.Test.Helpers;

public class DbContextHelper
{
    public static IApplicationDbContext GetContext()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        ApplicationDbContext context = new(options);
        SeedData(context);

        return context;
    }

    private static void SeedData(IApplicationDbContext context)
    {
        var sistemaId = Guid.NewGuid();
        context.Sistemas.Add(MockEntityHelper.GetNewSistema(sistemaId));
        context.Sistemas.Add(MockEntityHelper.GetNewSistema());
        context.Sistemas.Add(MockEntityHelper.GetNewSistema());

        var projetoId = Guid.NewGuid();
        context.Projetos.Add(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));
        context.Projetos.Add(MockEntityHelper.GetNewProjeto(sistemaId));
        context.Projetos.Add(MockEntityHelper.GetNewProjeto(sistemaId));

        var workflowId = Guid.NewGuid();
        context.Workflows.Add(MockEntityHelper.GetNewWorkflow("Projeto", 1, workflowId));
        context.Workflows.Add(MockEntityHelper.GetNewWorkflow("Projeto", 1));
        context.Workflows.Add(MockEntityHelper.GetNewWorkflow("Projeto", 1));

        var impedimentoId = Guid.NewGuid();
        context.Impedimentos.Add(MockEntityHelper.GetNewImpedimento(impedimentoId));
        context.Impedimentos.Add(MockEntityHelper.GetNewImpedimento());
        context.Impedimentos.Add(MockEntityHelper.GetNewImpedimento());

        var recursoId = Guid.NewGuid();
        context.Recursos.Add(MockEntityHelper.GetNewRecurso(recursoId));
        context.Recursos.Add(MockEntityHelper.GetNewRecurso());
        context.Recursos.Add(MockEntityHelper.GetNewRecurso());

        var tipoTarefaId = Guid.NewGuid();
        context.TipoTarefas.Add(MockEntityHelper.GetNewTipoTarefa("Projeto", "../gif/gif-projeto.gif", tipoTarefaId));
        context.TipoTarefas.Add(MockEntityHelper.GetNewTipoTarefa("Projeto", "../gif/gif-projeto.gif"));
        context.TipoTarefas.Add(MockEntityHelper.GetNewTipoTarefa("Projeto", "../gif/gif-projeto.gif"));

        var tarefaId = Guid.NewGuid();
        context.Tarefas.Add(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId));
        context.Tarefas.Add(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId));
        context.Tarefas.Add(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId));

        var apontamentoId = Guid.NewGuid();
        context.Apontamentos.Add(MockEntityHelper.GetNewApontamento(tarefaId, recursoId, apontamentoId));
        context.Apontamentos.Add(MockEntityHelper.GetNewApontamento(tarefaId, recursoId));
        context.Apontamentos.Add(MockEntityHelper.GetNewApontamento(tarefaId, recursoId));

        var impedimentoTarefaId = Guid.NewGuid();
        context.ImpedimentoTarefas.Add(MockEntityHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId, impedimentoTarefaId));
        context.ImpedimentoTarefas.Add(MockEntityHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId));
        context.ImpedimentoTarefas.Add(MockEntityHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId));

        var recursoProjetoId = Guid.NewGuid();
        context.RecursoProjetos.Add(MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId, recursoProjetoId));
        context.RecursoProjetos.Add(MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId));
        context.RecursoProjetos.Add(MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId));

        var recursoTarefaId = Guid.NewGuid();
        context.RecursoTarefas.Add(MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId, recursoTarefaId));
        context.RecursoTarefas.Add(MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId));
        context.RecursoTarefas.Add(MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId));

        context.SaveChangesAsync();
    }
}
