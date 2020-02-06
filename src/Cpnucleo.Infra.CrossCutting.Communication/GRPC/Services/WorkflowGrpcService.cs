using AutoMapper;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Protos;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
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
            List<WorkflowViewModel> result = new List<WorkflowViewModel>();

            using (AsyncServerStreamingCall<WorkflowModel> reply = _client.Listar(new Empty()))
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    result.Add(_mapper.Map<WorkflowViewModel>(reply.ResponseStream.Current));
                }
            }

            return result;
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
            List<WorkflowViewModel> result = new List<WorkflowViewModel>();

            using (AsyncServerStreamingCall<WorkflowModel> reply = _client.ListarPorTarefa(new Empty()))
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    result.Add(_mapper.Map<WorkflowViewModel>(reply.ResponseStream.Current));
                }
            }

            return result;
        }
    }
}
