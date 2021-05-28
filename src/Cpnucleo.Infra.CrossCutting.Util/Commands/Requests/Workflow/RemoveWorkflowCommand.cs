using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Workflow;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Workflow
{
    [DataContract]
    public class RemoveWorkflowCommand : IRequest<RemoveWorkflowResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
