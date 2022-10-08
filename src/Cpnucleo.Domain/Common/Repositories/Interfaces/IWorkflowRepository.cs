namespace Cpnucleo.Domain.Common.Repositories.Interfaces;

public interface IWorkflowRepository : IGenericRepository<Workflow>
{
    Task<int> GetQuantidadeColunasAsync();
}
