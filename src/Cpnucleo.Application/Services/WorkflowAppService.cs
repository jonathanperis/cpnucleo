using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class WorkflowAppService : CrudAppService<Workflow, WorkflowViewModel>, IWorkflowAppService
    {
        public WorkflowAppService(IMapper mapper, ICrudRepository<Workflow> repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {

        }

        public new WorkflowViewModel Consultar(Guid id)
        {
            WorkflowViewModel result = base.Consultar(id);

            int quantidadeColunas = ObterQuantidadeColunas();

            result.TamanhoColuna = ObterTamanhoColuna(quantidadeColunas);

            return result;
        }

        public new IEnumerable<WorkflowViewModel> Listar()
        {
            IEnumerable<WorkflowViewModel> result = base.Listar();
            
            int quantidadeColunas = ObterQuantidadeColunas();
            
            foreach (WorkflowViewModel item in result)
            {
                item.TamanhoColuna = ObterTamanhoColuna(quantidadeColunas);
            }

            return result;
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
