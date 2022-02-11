namespace Cpnucleo.Application.Common.Repositories.Interfaces;

public interface IWorkflowRepository : IGenericRepository<Workflow>
{
    Task<int> GetQuantidadeColunasAsync();

    string GetTamanhoColuna(int quantidadeColunas);
}
