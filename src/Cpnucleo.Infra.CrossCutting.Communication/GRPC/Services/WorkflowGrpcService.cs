using AutoMapper;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Services
{
    public class WorkflowGrpcService : BaseGrpcService, IWorkflowGrpcService
    {
        private readonly Workflow.WorkflowClient _client;

        public WorkflowGrpcService(IMapper mapper)
            : base(mapper)
        {
            _client = new Workflow.WorkflowClient(_channel);
        }

        public async Task<bool> IncluirAsync(WorkflowViewModel obj)
        {
            BaseReply reply = await _client.IncluirAsync(_mapper.Map<WorkflowModel>(obj));

            return reply.Sucesso;
        }

        public async Task<WorkflowViewModel> ConsultarAsync(Guid id)
        {
            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            return _mapper.Map<WorkflowViewModel>(await _client.ConsultarAsync(request));
        }

        public async Task<IEnumerable<WorkflowViewModel>> ListarAsync()
        {
            ListarReply response = await _client.ListarAsync(new Empty());
            return _mapper.Map<IEnumerable<WorkflowViewModel>>(response.Lista);
        }

        public async Task<bool> AlterarAsync(WorkflowViewModel obj)
        {
            BaseReply reply = await _client.AlterarAsync(_mapper.Map<WorkflowModel>(obj));

            return reply.Sucesso;
        }

        public async Task<bool> RemoverAsync(Guid id)
        {
            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            BaseReply reply = await _client.RemoverAsync(request);

            return reply.Sucesso;
        }

        public async Task<IEnumerable<WorkflowViewModel>> ListarPorTarefaAsync()
        {
            ListarPorTarefaReply response = await _client.ListarPorTarefaAsync(new Empty());
            return _mapper.Map<IEnumerable<WorkflowViewModel>>(response.Lista);
        }
    }
}
