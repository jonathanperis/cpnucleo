using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.RemoveWorkflow
{
    [DataContract]
    public class RemoveWorkflowCommand : IRequest<RemoveWorkflowResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
