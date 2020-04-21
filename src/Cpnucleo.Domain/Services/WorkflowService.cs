using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Domain.Services
{
    internal class WorkflowService : CrudService<Workflow>, IWorkflowService
    {
        public WorkflowService(ICrudRepository<Workflow> workflowRepository, IUnitOfWork unitOfWork)
            : base(workflowRepository, unitOfWork)
        {

        }

        public new IEnumerable<Workflow> Listar()
        {
            IEnumerable<Workflow> lista = base.Listar();

            int quantidadeColunas = ObterQuantidadeColunas();

            foreach (Workflow item in lista)
            {
                item.TamanhoColuna = ObterTamanhoColuna(quantidadeColunas);
            }

            return lista;
        }

        public new Workflow Consultar(Guid id)
        {
            Workflow workflow = base.Consultar(id);

            int quantidadeColunas = ObterQuantidadeColunas();

            workflow.TamanhoColuna = ObterTamanhoColuna(quantidadeColunas);

            return workflow;
        }

        public int ObterQuantidadeColunas()
        {
            return base.Listar().Count();
        }

        public string ObterTamanhoColuna(int quantidadeColunas)
        {
            quantidadeColunas = quantidadeColunas == 1 ? 2 : quantidadeColunas;

            int i = 12 / quantidadeColunas;
            return i.ToString();
        }
    }
}
