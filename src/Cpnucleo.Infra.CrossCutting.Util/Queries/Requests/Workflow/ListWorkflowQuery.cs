using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Workflow;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Workflow
{
    [DataContract]
    public class ListWorkflowQuery : IRequest<ListWorkflowResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
