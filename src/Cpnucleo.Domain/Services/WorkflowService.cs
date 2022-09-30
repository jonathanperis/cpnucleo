using Cpnucleo.Domain.Services.Interfaces;

namespace Cpnucleo.Domain.Services;

public sealed class WorkflowService : IWorkflowService
{
    public string GetTamanhoColuna(int colunas)
    {
        colunas = colunas == 1 ? 2 : colunas;

        int i = 12 / colunas;
        return i.ToString();
    }
}