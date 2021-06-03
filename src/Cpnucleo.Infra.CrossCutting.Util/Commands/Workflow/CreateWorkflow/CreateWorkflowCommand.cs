using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.CreateWorkflow
{
    [DataContract]
    public class CreateWorkflowCommand : IRequest<CreateWorkflowResponse>
    {
        [DataMember(Order = 1)]
        public WorkflowViewModel Workflow { get; set; }
    }
}
