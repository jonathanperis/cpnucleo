using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Domain.Interfaces.Services;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Services
{
    public class TarefaService : CrudService<Tarefa>, ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IWorkflowService _workflowService;
        private readonly IApontamentoService _apontamentoService;
        private readonly IImpedimentoTarefaService _impedimentoTarefaService;

        public TarefaService(ITarefaRepository tarefaRepository, IUnitOfWork unitOfWork, IWorkflowService workflowService, IApontamentoService apontamentoService, IImpedimentoTarefaService impedimentoTarefaService)
            : base(tarefaRepository, unitOfWork)
        {
            _tarefaRepository = tarefaRepository;
            _workflowService = workflowService;
            _apontamentoService = apontamentoService;
            _impedimentoTarefaService = impedimentoTarefaService;
        }

        public new IQueryable<Tarefa> Listar()
        {
            IQueryable<Tarefa> lista = base.Listar();

            return PreencherDadosAdicionais(lista);
        }

        public IQueryable<Tarefa> ListarPorRecurso(Guid idRecurso)
        {
            IQueryable<Tarefa> lista = _tarefaRepository.ListarPorRecurso(idRecurso);

            return PreencherDadosAdicionais(lista);
        }

        public bool AlterarPorWorkflow(Guid idTarefa, Guid idWorkflow)
        {
            lock (this)
            {
                Tarefa tarefa = Consultar(idTarefa).FirstOrDefault();
                Workflow workflow = _workflowService.Consultar(idWorkflow).FirstOrDefault();

                tarefa.IdWorkflow = idWorkflow;
                tarefa.Workflow = workflow;

                return Alterar(tarefa);
            }
        }

        private IQueryable<Tarefa> PreencherDadosAdicionais(IQueryable<Tarefa> lista)
        {
            int quantidadeColunas = _workflowService.ObterQuantidadeColunas();

            foreach (Tarefa item in lista)
            {
                item.Workflow.TamanhoColuna = _workflowService.ObterTamanhoColuna(quantidadeColunas);
                item.HorasConsumidas = _apontamentoService.ObterTotalHorasPorRecurso(item.IdRecurso, item.Id);
                item.HorasRestantes = item.QtdHoras - item.HorasConsumidas;

                if (_impedimentoTarefaService.ListarPorTarefa(item.Id).Count() > 0)
                {
                    item.TipoTarefa.Element = "warning-element";
                }
                else if (DateTime.Now.Date >= item.DataInicio && DateTime.Now.Date <= item.DataTermino)
                {
                    item.TipoTarefa.Element = "success-element";
                }
                else if (DateTime.Now.Date > item.DataTermino)
                {
                    item.TipoTarefa.Element = "danger-element";
                }
                else
                {
                    item.TipoTarefa.Element = "info-element";
                }
            }

            return lista;
        }
    }
}
