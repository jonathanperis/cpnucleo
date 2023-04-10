namespace Cpnucleo.Application.Test.Helpers;

public class DbContextHelper
{
    public static IApplicationDbContext GetContext()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        ApplicationDbContext context = new(options);
        context.SaveChanges();

        return context;
    }

    public static async Task SeedData(IApplicationDbContext context)
    {
        Guid sistemaId = Guid.NewGuid();
        await context.Sistemas.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));
        await context.Sistemas.AddAsync(MockEntityHelper.GetNewSistema());
        await context.Sistemas.AddAsync(MockEntityHelper.GetNewSistema());

        Guid projetoId = Guid.NewGuid();
        await context.Projetos.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));
        await context.Projetos.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId));
        await context.Projetos.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId));

        Guid workflowId = Guid.NewGuid();
        await context.Workflows.AddAsync(MockEntityHelper.GetNewWorkflow("Projeto", 1, workflowId));
        await context.Workflows.AddAsync(MockEntityHelper.GetNewWorkflow("Projeto", 1));
        await context.Workflows.AddAsync(MockEntityHelper.GetNewWorkflow("Projeto", 1));

        Guid impedimentoId = Guid.NewGuid();
        await context.Impedimentos.AddAsync(MockEntityHelper.GetNewImpedimento(impedimentoId));
        await context.Impedimentos.AddAsync(MockEntityHelper.GetNewImpedimento());
        await context.Impedimentos.AddAsync(MockEntityHelper.GetNewImpedimento());

        Guid recursoId = Guid.NewGuid();
        await context.Recursos.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));
        await context.Recursos.AddAsync(MockEntityHelper.GetNewRecurso());
        await context.Recursos.AddAsync(MockEntityHelper.GetNewRecurso());

        Guid tipoTarefaId = Guid.NewGuid();
        await context.TipoTarefas.AddAsync(MockEntityHelper.GetNewTipoTarefa("Projeto", "../gif/gif-projeto.gif", tipoTarefaId));
        await context.TipoTarefas.AddAsync(MockEntityHelper.GetNewTipoTarefa("Projeto", "../gif/gif-projeto.gif"));
        await context.TipoTarefas.AddAsync(MockEntityHelper.GetNewTipoTarefa("Projeto", "../gif/gif-projeto.gif"));

        Guid tarefaId = Guid.NewGuid();
        await context.Tarefas.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId));
        await context.Tarefas.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId));
        await context.Tarefas.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId));

        Guid apontamentoId = Guid.NewGuid();
        await context.Apontamentos.AddAsync(MockEntityHelper.GetNewApontamento(tarefaId, recursoId, apontamentoId));
        await context.Apontamentos.AddAsync(MockEntityHelper.GetNewApontamento(tarefaId, recursoId));
        await context.Apontamentos.AddAsync(MockEntityHelper.GetNewApontamento(tarefaId, recursoId));

        Guid impedimentoTarefaId = Guid.NewGuid();
        await context.ImpedimentoTarefas.AddAsync(MockEntityHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId, impedimentoTarefaId));
        await context.ImpedimentoTarefas.AddAsync(MockEntityHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId));
        await context.ImpedimentoTarefas.AddAsync(MockEntityHelper.GetNewImpedimentoTarefa(tarefaId, impedimentoId));

        Guid recursoProjetoId = Guid.NewGuid();
        await context.RecursoProjetos.AddAsync(MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId, recursoProjetoId));
        await context.RecursoProjetos.AddAsync(MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId));
        await context.RecursoProjetos.AddAsync(MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId));

        Guid recursoTarefaId = Guid.NewGuid();
        await context.RecursoTarefas.AddAsync(MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId, recursoTarefaId));
        await context.RecursoTarefas.AddAsync(MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId));
        await context.RecursoTarefas.AddAsync(MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId));

        await context.SaveChangesAsync();
    }
}
