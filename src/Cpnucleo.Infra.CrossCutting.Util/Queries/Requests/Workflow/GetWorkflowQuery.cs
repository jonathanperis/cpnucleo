using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Workflow;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Workflow
{
    [DataContract]
    public class GetWorkflowQuery : IRequest<GetWorkflowResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
