using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.CreateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.RemoveWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.UpdateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;
using Cpnucleo.MVC.Interfaces;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Client;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Services
{
    internal class WorkflowService : GrpcService, IWorkflowService
    {
        private IWorkflowGrpcService _WorkflowGrpcService;

        public WorkflowService(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<CreateWorkflowResponse> AddAsync(string token, CreateWorkflowCommand command)
        {
            _WorkflowGrpcService = InitializeAuthenticatedChannel(token);
            return await _WorkflowGrpcService.AddAsync(command);
        }

        public async Task<ListWorkflowResponse> AllAsync(string token, ListWorkflowQuery query)
        {
            _WorkflowGrpcService = InitializeAuthenticatedChannel(token);
            return await _WorkflowGrpcService.AllAsync(query);
        }

        public async Task<GetWorkflowResponse> GetAsync(string token, GetWorkflowQuery query)
        {
            _WorkflowGrpcService = InitializeAuthenticatedChannel(token);
            return await _WorkflowGrpcService.GetAsync(query);
        }

        public async Task<RemoveWorkflowResponse> RemoveAsync(string token, RemoveWorkflowCommand command)
        {
            _WorkflowGrpcService = InitializeAuthenticatedChannel(token);
            return await _WorkflowGrpcService.RemoveAsync(command);
        }

        public async Task<UpdateWorkflowResponse> UpdateAsync(string token, UpdateWorkflowCommand command)
        {
            _WorkflowGrpcService = InitializeAuthenticatedChannel(token);
            return await _WorkflowGrpcService.UpdateAsync(command);
        }

        private IWorkflowGrpcService InitializeAuthenticatedChannel(string token)
        {
            _channel = CreateAuthenticatedChannel(token);
            return _channel.CreateGrpcService<IWorkflowGrpcService>();
        }
    }
}