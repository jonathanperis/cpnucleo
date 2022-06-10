namespace Cpnucleo.Infra.Data.Repositories;

internal class WorkflowRepository : GenericRepository<Workflow>, IWorkflowRepository
{
    public WorkflowRepository(CpnucleoContext context)
        : base(context) { }

    public async Task<int> GetQuantidadeColunasAsync()
    {
        Expression<Func<Workflow, bool>> predicate = x => x.Ativo;

        return await All(predicate, true)
            .CountAsync();
    }
}
