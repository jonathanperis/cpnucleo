using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Domain.Services
{
    internal class TarefaService : CrudService<Tarefa>, ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IWorkflowService _workflowService;
        private readonly IApontamentoService _apontamentoService;
        private readonly IImpedimentoTarefaService _impedimentoTarefaService;
        private readonly ICrudService<TipoTarefa> _tipoTarefaService;

        public TarefaService(ITarefaRepository tarefaRepository, IUnitOfWork unitOfWork, IWorkflowService workflowService, IApontamentoService apontamentoService, IImpedimentoTarefaService impedimentoTarefaService, ICrudService<TipoTarefa> tipoTarefaService)
            : base(tarefaRepository, unitOfWork)
        {
            _tarefaRepository = tarefaRepository;
            _workflowService = workflowService;
            _apontamentoService = apontamentoService;
            _impedimentoTarefaService = impedimentoTarefaService;
            _tipoTarefaService = tipoTarefaService;
        }

        public new IEnumerable<Tarefa> Listar()
        {
            IEnumerable<Tarefa> lista = base.Listar();

            return PreencherDadosAdicionais(lista);
        }

        public IEnumerable<Tarefa> ListarPorRecurso(Guid idRecurso)
        {
            IEnumerable<Tarefa> lista = _tarefaRepository.ListarPorRecurso(idRecurso);

            return PreencherDadosAdicionais(lista);
        }

        public bool AlterarPorWorkflow(Guid idTarefa, Guid idWorkflow)
        {
            lock (this)
            {
                Tarefa tarefa = Consultar(idTarefa);
                Workflow workflow = _workflowService.Consultar(idWorkflow);

                tarefa.IdWorkflow = idWorkflow;
                tarefa.Workflow = workflow;

                return Alterar(tarefa);
            }
        }

        private IEnumerable<Tarefa> PreencherDadosAdicionais(IEnumerable<Tarefa> lista)
        {
            int quantidadeColunas = _workflowService.ObterQuantidadeColunas();

            foreach (Tarefa item in lista)
            {
                item.Workflow = _workflowService.Consultar(item.IdWorkflow);
                item.Workflow.TamanhoColuna = _workflowService.ObterTamanhoColuna(quantidadeColunas);
                
                item.HorasConsumidas = _apontamentoService.ObterTotalHorasPorRecurso(item.IdRecurso, item.Id);
                item.HorasRestantes = item.QtdHoras - item.HorasConsumidas;

                item.TipoTarefa = _tipoTarefaService.Consultar(item.IdTipoTarefa);

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
