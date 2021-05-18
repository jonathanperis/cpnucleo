using AutoMapper;
using Cpnucleo.Domain.UoW;
using Cpnucleo.GRPC.Protos;
using Cpnucleo.GRPC.Protos.TarefaProto;
using Cpnucleo.Domain.Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Google.Protobuf.Collections;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.GRPC
{
    [Authorize]    
    public class TarefaService : TarefaProto.TarefaProtoBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TarefaService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public override async Task<TarefaModel> Incluir(TarefaModel request, ServerCallContext context)
        {
            TarefaModel result = _mapper.Map<TarefaModel>(_mapper.Map<Apontamento>(request));

            return await Task.FromResult(result);
        }

        public override async Task<ListarReply> Listar(Empty request, ServerCallContext context)
        {
            ListarReply result = new ListarReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<TarefaModel>>(_unitOfWork.TarefaRepository.All(true)));

            PreencherDadosAdicionais(result.Lista);

            return await Task.FromResult(result);
        }

        public override async Task<TarefaModel> Consultar(BaseRequest request, ServerCallContext context)
        {
            Guid id = new Guid(request.Id);
            TarefaModel result = _mapper.Map<TarefaModel>(_unitOfWork.TarefaRepository.Get(id));

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> Alterar(TarefaModel request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.TarefaRepository.Update(_mapper.Map<Tarefa>(request))
            });
        }

        public override async Task<BaseReply> Remover(BaseRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.TarefaRepository.Remove(new Guid(request.Id))
            });
        }

        public override async Task<ListarPorRecursoReply> ListarPorRecurso(BaseRequest request, ServerCallContext context)
        {
            ListarPorRecursoReply result = new ListarPorRecursoReply();
            result.Lista.AddRange(_mapper.Map<IEnumerable<TarefaModel>>(_unitOfWork.TarefaRepository.GetByRecurso(new Guid(request.Id))));

            PreencherDadosAdicionais(result.Lista);

            return await Task.FromResult(result);
        }

        public override async Task<BaseReply> AlterarPorWorkflow(AlterarPorWorkflowRequest request, ServerCallContext context)
        {
            Guid idTarefa = new Guid(request.IdTarefa);
            Guid idWorkflow = new Guid(request.IdWorkflow);

            Tarefa tarefa = _unitOfWork.TarefaRepository.Get(idTarefa);
            Workflow workflow = _unitOfWork.WorkflowRepository.Get(idWorkflow);

            tarefa.IdWorkflow = idWorkflow;
            tarefa.Workflow = workflow;

            return await Task.FromResult(new BaseReply
            {
                Sucesso = _unitOfWork.TarefaRepository.Update(tarefa)
            });
        }

        private RepeatedField<TarefaModel> PreencherDadosAdicionais(RepeatedField<TarefaModel> lista)
        {
            int colunas = _unitOfWork.WorkflowRepository.GetQuantidadeColunas();

            foreach (TarefaModel item in lista)
            {
                Guid idTarefa = new Guid(item.Id);
                Guid idRecurso = new Guid(item.IdRecurso);

                item.Workflow.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);
                
                item.HorasConsumidas = _unitOfWork.ApontamentoRepository.GetTotalHorasPorRecurso(idRecurso, idTarefa);
                item.HorasRestantes = item.QtdHoras - item.HorasConsumidas;

                DateTime dataInicio;
                DateTime.TryParse(item.DataInicio, out dataInicio);

                DateTime dataTermino;
                DateTime.TryParse(item.DataTermino, out dataTermino);

                if (_unitOfWork.ImpedimentoTarefaRepository.GetByTarefa(idTarefa).Count() > 0)
                {
                    item.TipoTarefa.Element = "warning-element";
                }
                else if (DateTime.Now.Date >= dataInicio && DateTime.Now.Date <= dataTermino)
                {
                    item.TipoTarefa.Element = "success-element";
                }
                else if (DateTime.Now.Date > dataTermino)
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
