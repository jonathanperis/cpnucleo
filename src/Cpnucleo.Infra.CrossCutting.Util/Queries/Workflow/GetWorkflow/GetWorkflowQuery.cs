using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow
{
    [DataContract]
    public class GetWorkflowQuery : IRequest<GetWorkflowResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
