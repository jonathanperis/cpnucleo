using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class WorkflowAppService : CrudAppService<Workflow, WorkflowViewModel>, IWorkflowAppService
    {
        protected readonly IWorkflowRepository _workflowRepository;

        public WorkflowAppService(IMapper mapper, ICrudRepository<Workflow> repository, IUnitOfWork unitOfWork, IWorkflowRepository workflowRepository)
            : base(mapper, repository, unitOfWork)
        {
            _workflowRepository = workflowRepository;
        }

        public IEnumerable<WorkflowViewModel> ListarPorTarefa()
        {
            IEnumerable<WorkflowViewModel> listaWorkflow = _mapper.Map<IEnumerable<WorkflowViewModel>>(_workflowRepository.ListarPorTarefa());

            foreach (WorkflowViewModel item in listaWorkflow)
            {
                int qtdLista = listaWorkflow.Count();
                qtdLista = qtdLista == 1 ? 2 : qtdLista;

                int i = 12 / qtdLista;
                item.TamanhoColuna = i;

                foreach (TarefaViewModel tarefa in item.ListaTarefas)
                {
                    tarefa.HorasConsumidas = tarefa.ListaApontamentos.Sum(x => x.QtdHoras);
                    tarefa.HorasRestantes = tarefa.QtdHoras - tarefa.HorasConsumidas;

                    if (DateTime.Now.Date >= tarefa.DataInicio && DateTime.Now.Date <= tarefa.DataTermino)
                    {
                        tarefa.TipoTarefa.Element = "success-element";
                    }
                    else if (tarefa.ListaImpedimentos.Count(x => x.Ativo) > 0)
                    {
                        tarefa.TipoTarefa.Element = "warning-element";
                    }
                    else if (DateTime.Now.Date > tarefa.DataTermino && tarefa.PercentualConcluido != 100)
                    {
                        tarefa.TipoTarefa.Element = "danger-element";
                    }
                    else
                    {
                        tarefa.TipoTarefa.Element = "info-element";
                    }
                }
            }

            return listaWorkflow;
        }
    }
}
