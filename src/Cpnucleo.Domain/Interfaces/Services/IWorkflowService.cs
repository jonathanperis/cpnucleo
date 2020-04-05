using Cpnucleo.Domain.Entities;

namespace Cpnucleo.Domain.Interfaces.Services
{
    public interface IWorkflowService : ICrudService<Workflow>
    {
        int ObterQuantidadeColunas();

        string ObterTamanhoColuna(int quantidadeColunas);
    }
}
