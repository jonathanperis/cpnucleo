using AutoMapper;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Handlers
{
    internal class TarefaHandler :
        IRequestHandler<CreateTarefaCommand, CreateTarefaResponse>,
        IRequestHandler<GetTarefaQuery, GetTarefaResponse>,
        IRequestHandler<ListTarefaQuery, ListTarefaResponse>,
        IRequestHandler<RemoveTarefaCommand, RemoveTarefaResponse>,
        IRequestHandler<UpdateTarefaCommand, UpdateTarefaResponse>,
        IRequestHandler<GetByRecursoQuery, GetByRecursoResponse>,
        IRequestHandler<UpdateByWorkflowCommand, UpdateByWorkflowResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateTarefaResponse> Handle(CreateTarefaCommand request, CancellationToken cancellationToken)
        {
            CreateTarefaResponse result = new CreateTarefaResponse
            {
                Status = OperationResult.Failed
            };

            Tarefa obj = await _unitOfWork.TarefaRepository.AddAsync(_mapper.Map<Tarefa>(request.Tarefa));
            result.Tarefa = _mapper.Map<TarefaViewModel>(obj);

            await _unitOfWork.SaveChangesAsync();

            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<GetTarefaResponse> Handle(GetTarefaQuery request, CancellationToken cancellationToken)
        {
            GetTarefaResponse result = new GetTarefaResponse
            {
                Status = OperationResult.Failed
            };

            result.Tarefa = _mapper.Map<TarefaViewModel>(await _unitOfWork.TarefaRepository.GetAsync(request.Id));
            result.Status = OperationResult.Success;

            return result;
        }

        public async Task<ListTarefaResponse> Handle(ListTarefaQuery request, CancellationToken cancellationToken)
        {
            ListTarefaResponse result = new ListTarefaResponse
            {
                Status = OperationResult.Failed
            };

            result.Tarefas = _mapper.Map<IEnumerable<TarefaViewModel>>(await _unitOfWork.TarefaRepository.AllAsync(request.GetDependencies));
            result.Status = OperationResult.Success;

            await PreencherDadosAdicionaisAsync(result.Tarefas);

            return result;
        }

        public async Task<RemoveTarefaResponse> Handle(RemoveTarefaCommand request, CancellationToken cancellationToken)
        {
            RemoveTarefaResponse result = new RemoveTarefaResponse
            {
                Status = OperationResult.Failed
            };

            Tarefa obj = await _unitOfWork.TarefaRepository.GetAsync(request.Id);

            if (obj == null)
            {
                return null;
            }

            await _unitOfWork.TarefaRepository.RemoveAsync(request.Id);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<UpdateTarefaResponse> Handle(UpdateTarefaCommand request, CancellationToken cancellationToken)
        {
            UpdateTarefaResponse result = new UpdateTarefaResponse
            {
                Status = OperationResult.Failed
            };

            _unitOfWork.TarefaRepository.Update(_mapper.Map<Tarefa>(request.Tarefa));

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        public async Task<GetByRecursoResponse> Handle(GetByRecursoQuery request, CancellationToken cancellationToken)
        {
            GetByRecursoResponse result = new GetByRecursoResponse
            {
                Status = OperationResult.Failed
            };

            result.Tarefas = _mapper.Map<IEnumerable<TarefaViewModel>>(await _unitOfWork.TarefaRepository.GetByRecursoAsync(request.IdRecurso));
            result.Status = OperationResult.Success;

            await PreencherDadosAdicionaisAsync(result.Tarefas);

            return result;
        }

        public async Task<UpdateByWorkflowResponse> Handle(UpdateByWorkflowCommand request, CancellationToken cancellationToken)
        {
            UpdateByWorkflowResponse result = new UpdateByWorkflowResponse
            {
                Status = OperationResult.Failed
            };

            Tarefa tarefa = await _unitOfWork.TarefaRepository.GetAsync(request.IdTarefa);

            tarefa.IdWorkflow = request.Workflow.Id;
            tarefa.Workflow = _mapper.Map<Workflow>(request.Workflow); //TODO: - VERIFICAR NECESSIDADE.

            _unitOfWork.TarefaRepository.Update(tarefa);

            bool success = await _unitOfWork.SaveChangesAsync();

            result.Status = success ? OperationResult.Success : OperationResult.Failed;

            return result;
        }

        private async Task PreencherDadosAdicionaisAsync(IEnumerable<TarefaViewModel> lista)
        {
            int colunas = await _unitOfWork.WorkflowRepository.GetQuantidadeColunasAsync();

            foreach (TarefaViewModel item in lista)
            {
                item.Workflow.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);

                item.HorasConsumidas = await _unitOfWork.ApontamentoRepository.GetTotalHorasPorRecursoAsync(item.IdRecurso, item.Id);
                item.HorasRestantes = item.QtdHoras - item.HorasConsumidas;

                IEnumerable<ImpedimentoTarefa> impedimentos = await _unitOfWork.ImpedimentoTarefaRepository.GetByTarefaAsync(item.Id);

                if (impedimentos.Any())
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
        }
    }
}
