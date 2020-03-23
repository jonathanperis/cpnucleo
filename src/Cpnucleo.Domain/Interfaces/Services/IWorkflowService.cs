using Cpnucleo.Domain.Entities;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Interfaces.Services
{
    public interface IWorkflowService : ICrudService<Workflow>
    {
        int ObterQuantidadeColunas();

        string ObterTamanhoColuna(int quantidadeColunas);

        IQueryable<Workflow> ObterPorTarefa(Guid idTarefa);
    }
}
